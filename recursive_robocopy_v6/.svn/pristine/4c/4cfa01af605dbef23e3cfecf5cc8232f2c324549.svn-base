using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using recursive_robocopy.classes;

namespace recursive_robocopy
{
    /// <summary>
    ///   encapsulate the full robocopy process
    /// </summary>
    class recursiveRobocopyProgram
    {
        private context _myContext;
        
        public context myContext
        {
            get { return _myContext; }
        }

        /// <summary>
        ///   base "program" to start running the process <see cref="System.Console.WriteLine(System.String)"/> - <para>see <see cref="recursive_robocopy.classes.RobocopyTask.doRoboCopy()"/> for the algorithm and logic implementation</para>
        ///   
        /// </summary>
        public void startRoboCopy()
        {
            System.Console.Out.WriteLine("START - Recursive robocopy");
            
            // validate folders
            bool hasErrors = false;
            //// basics - neither should be blank
            // source - exists
            if (!System.IO.Directory.Exists(myContext.Folders.SourceFolder))
            {
                hasErrors = true;
                System.Console.Out.WriteLine(String.Format("ERROR: Source directory does not exist ({0})", myContext.Folders.SourceFolder));

            }
            // target - can be created (mostly access tests)
            try
            {
                // create the target directory
                System.IO.Directory.CreateDirectory(myContext.Folders.TargetFolder);
            }
            catch (IOException ioe)
            {
                //directory cannot be created - record and log details to console
                hasErrors = true;
                System.Console.Out.WriteLine(String.Format("ERROR: Target directory could not be created - Detail: ({0}) Dir: {1}", ioe.Message, myContext.Folders.TargetFolder));
                consoleHelper.WriteUsage();
            }

            if (hasErrors) // stop process on errors
                return;

            // TODO report start & implement all logging

            // create and queue the top-level task
            var robocopytask = new RobocopyTask(myContext.Folders);

            // setup the countdownevent object and pass it in as state
            var cde = new CountdownEvent(1);
            var cdeActiveTasks = new CountdownEvent(1);
            var semOutput = new Semaphore(1, 1); // semaphore to control output

            var myState = new doRoboCopyState(cde, cdeActiveTasks, semOutput , true); 

            System.Console.Out.WriteLine(string.Format("QUEUED - {0}",myContext.Folders.SourceFolder));

            // setup the thread pool
            int maxThreads, maxPortThreads;
            int localmaxThreads, localmaxPortThreads;
            int minThreads, minPortThreads;

            ThreadPool.GetMinThreads(out minThreads, out minPortThreads);
            ThreadPool.GetMaxThreads(out maxThreads, out maxPortThreads);

            // set max threads from the config or defaults if missing
            ThreadPool.SetMinThreads(1, minPortThreads);
            ThreadPool.SetMaxThreads((int)ConfigDefaults.getMaxThreads(), maxPortThreads);  // set thread pool limits - don't touch the local port max
            
            ThreadPool.GetMaxThreads(out localmaxThreads, out localmaxPortThreads);
            ThreadPool.GetMinThreads(out minThreads, out minPortThreads);
            
            System.Console.Out.WriteLine(string.Format("  Thread Pool - Min {0} max {1} local max {2}", minThreads,maxThreads,localmaxThreads));
            ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(robocopytask.doRoboCopy),myState);
            
            // Wait for everything to finish
            cde.Wait();

            // report end - 
            System.Console.Out.WriteLine("END - Recursive robocopy");
        }



        public recursiveRobocopyProgram(string[] args)
        {
            // process parameters - process sub throws an argument exception - handled herein
            //  - behaviour is in keeping with most MS DOS commands - spit out the 
            //     details and provide a usage blurb on error
           bool hasErrors=false;
           try
            {
                
                directoryPair dirs = consoleHelper.processParameters(args);

                // create context - all OK with input parms if no exceptions thrown
                //  - directory validation occurs in the context
                _myContext = new context(args);
            }
            catch (ArgumentException ae) // handle bad arguements 
            {
                if (ae.ParamName==consoleHelper.kArgumentExceptionMessageUnknown)
                    System.Console.WriteLine("ERROR: Unkown Problem with parameters - illegal characters, maybe? ");
                else if (ae.ParamName==consoleHelper.kArgumentExceptionMessageSourceandTargetFolder)
                    System.Console.WriteLine("ERROR: Problem with parameters - None Provided!");
                else if (ae.ParamName==consoleHelper.kArgumentExceptionMessageTargetFolder)
                    System.Console.WriteLine("ERROR: Problem with parameters - Target folder not provided");
                // re-throw the exception
                throw ae;
            }
            catch (Exception ex) // handle unexpected exceptions
            {

                // unrecoverable - record that we had errors
                hasErrors = true;
                //TODO fixup handling of unexpected exceptions
                throw new NotImplementedException("Handling not implemented for unexpected exceptions", ex);
            }

            // output USAGE text to the console
            if (hasErrors)
                consoleHelper.WriteUsage();
        }
    }
}
