using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace recursive_robocopy.classes
{
    /// <summary>
    ///   helper class for dealing with parameters and for writing out the usage statement when the caller
    ///     gets things wrong.
    /// </summary>
    public class consoleHelper
    {
        public const string kArgumentExceptionMessageUnknown="Unkown";

        public const string kArgumentExceptionMessageTargetFolder="Target Folder";

        public const string kArgumentExceptionMessageSourceandTargetFolder="Source and Target Folder";

        public static directoryPair processParameters(string[] args)
        {
            directoryPair localPair = new directoryPair();
            string paramsMissing;

            if (args.Length < 2)
            {
                paramsMissing = kArgumentExceptionMessageUnknown; // you have a logical issue if you get this one !
                if (args.Length == 1)
                    paramsMissing = kArgumentExceptionMessageTargetFolder;
                else if (args.Length == 0)
                    paramsMissing = kArgumentExceptionMessageSourceandTargetFolder;
                // raise exception for missings parms - app will handle
                //TODO turn paramsMissing into an enum and change the processing code
                throw new ArgumentException("Invalid Arguement(s)", paramsMissing);
            }
            else
            {
                // all good - set the source and target folders based on the parms
                localPair.SourceFolder = args[0];
                localPair.TargetFolder = args[1];
            }
            return localPair;
        }

        internal static void WriteUsage()
        {
            System.Console.WriteLine("USAGE:  ** NONE *** TBD ****");
            //TODO: Lookup how to write a resource file and send to standard output
        }

        public static void ExecuteCommand(string command, TextWriter commandOutput)
        {
            ProcessStartInfo cmd = new ProcessStartInfo("cmd.exe");
            cmd.RedirectStandardInput = true;
            cmd.RedirectStandardOutput = commandOutput==null?false:true;
            cmd.UseShellExecute = false;
            cmd.CreateNoWindow = false;
            cmd.WindowStyle = ProcessWindowStyle.Normal;
            cmd.UseShellExecute = false;
            
            Process console = Process.Start(cmd);
                        
            console.StandardInput.WriteLine(command);
            console.StandardInput.WriteLine("EXIT");
            if (cmd.RedirectStandardOutput)
                commandOutput.Write(console.StandardOutput.ReadToEnd()); // this is how you do it - instance before the wait to exit
            console.WaitForExit();
        }

        /// <summary>
        ///   Default constructor - do not redirect stdout
        /// </summary>
        /// <param name="command"></param>
        /// <param name="commandOutputStream"></param>
        public static void ExecuteCommand(string command)
        {
            ExecuteCommand(command, null);
        }

        
    }
}
