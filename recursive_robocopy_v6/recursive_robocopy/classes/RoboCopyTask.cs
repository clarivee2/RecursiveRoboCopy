using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace recursive_robocopy.classes
{
    /// <summary>
    ///    encapsulate the robocopy and requeue sub dir levels
    ///       - start the process by submitting the top-level to the threadpool - this task does
    ///         the rest recursively
    /// </summary>
    class RobocopyTask : Task
    {

        private directoryPair _folders;

        public directoryPair Folders
        {
            get { return _folders; }
        }

        public string SourceFolder
        {
            get { return _folders.SourceFolder; }
        }

        public string TargetFolder
        {
            get { return _folders.TargetFolder; }
        }

        /// <summary>
        ///   robocopy cmd command - use string.format to substitute source and target folders-
        ///   - converted to private at V5 
        ///   
        /// </summary>
        private string robocopy_command = ConfigDefaults.getRobocopyCommand();

        public string Robocopy_Command
        {
            
            // add parenthese around the folders for compatibility with spaces
            get { return string.Format(robocopy_command, "\"" + SourceFolder + "\"", "\"" + TargetFolder + "\"",ConfigDefaults.numLevels(),ConfigDefaults.getFileExclusions() ); }
        }
        
        public string getRobocopy_Command_Raw
        {
            get { return robocopy_command; }
        }


        public IEnumerable<string> getNextLevelFolders(string inPath, int currLevel)
        {

            // test and recurse subdirs
            IEnumerable<string> subDirectories = Directory.EnumerateDirectories(inPath, "*");
            var tempDirList = new List<string>();
                        
            //loop thru each directory
            foreach (string currDir in subDirectories)
            {
               if (currLevel == 1)
                { 
                  // add directory to output list
                    tempDirList.Add(currDir);
                }
                else
                {
                    //recurse down a level
                    var inList = getNextLevelFolders(currDir, currLevel - 1);

                    // merge the returned list
                    if (inList.Count()>0)
                        tempDirList.AddRange(inList);
                }
            }
            return tempDirList;
          }



        /// <summary>
        /// execution sub - intended for submission to the threadpool
        ///   - passes out the reference to the task itself for state.
        /// </summary>
        /// <param name="state">cde - countdownevent for tracking process ends</param>
        public void doRoboCopy(object state)
        {
            // get state 
            doRoboCopyState myState = (doRoboCopyState)state;
            CountdownEvent cde = myState.Cde; // get countdownevent counter
            CountdownEvent cdeActiveTasks = myState.CdeActiveTasks; // get task control counter
            Semaphore semOutput = myState.semOutput; // get the output semaphore

            bool amLive = false;

            TextWriter outputStream;

            // increment active task counter
            cdeActiveTasks.AddCount();

            // check if you are the live process - not live if you have to wait more than one second
            amLive= semOutput.WaitOne(1000);

            if(amLive)
            {
               // live - set stream to standard out
                outputStream = System.Console.Out;
            }   
            else
            {
                // not live - use a text writer
                outputStream = new StringWriter();
            }

            outputStream.WriteLine(string.Format("START - robocopy {0} - active tasks {1}", SourceFolder,cdeActiveTasks.CurrentCount));

            try
            {
                
                // todo extract method (make recursive) to support levels >1

                // recurse directories - returns a list of next level directories based on the level config
                IEnumerable<string> directories = getNextLevelFolders(SourceFolder, ConfigDefaults.numLevels());

                foreach (string currDir in directories)
                {

                    string currTargetFolder = directoryHelper.computeTargetFolder(currDir, SourceFolder, TargetFolder);
                    // increment counter
                    cde.AddCount(1);
                    // queue item - set to sleep on exceptions
                    myState.SleepOnException = true;
                    var newTask = new RobocopyTask(currDir, currTargetFolder);
                    ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(newTask.doRoboCopy), myState);
                    outputStream.WriteLine(string.Format("  QUEUED - robocopy {0}", currTargetFolder));
                }
            }
            // make the process wait forever like robocopy - implement a max count someday
            catch (Exception e)
            {
                System.Console.Out.WriteLine(string.Format("  EXCEPTION - at directories {0}", e.Message));
                // sleep - if ok else exit
                if (myState.SleepOnException)
                {
                    System.Console.Out.WriteLine("App Waiting 30 seconds...");
                    Thread.Sleep(30000); // wait 30 seconds
                }
                else
                {
                    cde.Signal();
                    cdeActiveTasks.Signal();
                    return;
                }
            }

            // start the robocopy (1 level) - new
            //  NOTE: robocopy_command is a string-format implementation in a calculated property - quotes are added 
            //    to support the situation where the folders contain spaces
            //    - changed it to a config item in V5
            // point output to the local stream
             consoleHelper.ExecuteCommand(Robocopy_Command,outputStream);
            outputStream.WriteLine(string.Format("  DONE - copy {0}", SourceFolder));
 
            // decrement counters for this task
            cde.Signal();
            cdeActiveTasks.Signal();

            outputStream.WriteLine(string.Format("END - robocopy {0} - Task Count {1}", SourceFolder, cde.CurrentCount));
            // all done - let go of the semaphore
            if (amLive)
            {
                semOutput.Release();
            }
            else
            {
                // wait for the semaphore and stream the ouput
                if(semOutput.WaitOne())
                {
                    System.Console.Out.Write(outputStream.ToString());
                    semOutput.Release();
                }
            }

        }

        public RobocopyTask(directoryPair inFolders)
        {
            this._folders = inFolders;
        }

        public RobocopyTask(string sourcefolder, string targetfolder)
            : this(new directoryPair(sourcefolder, targetfolder))
        {

        }
    }
}
