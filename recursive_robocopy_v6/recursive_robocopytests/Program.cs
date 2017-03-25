using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using recursive_robocopy.classes;
using System.Threading;
using System.IO;

namespace recursive_robocopytests
{
    class Program
    {

        static void Main(string[] args)
        {

            //// system io test
            ////  - start a process , redirect output to a memory stream and then run the cmd and send that output to standard out
            //ProcessStartInfo cmd = new ProcessStartInfo("cmd.exe");
            //cmd.RedirectStandardInput = true;
            //cmd.RedirectStandardOutput = true;
            //cmd.UseShellExecute = false;
            //cmd.CreateNoWindow = false;
            //cmd.WindowStyle = ProcessWindowStyle.Normal;

            //Process console = Process.Start(cmd);
            //commandOutputStream = console.StandardOutput;
            //console.StandardInput.WriteLine(command);
            //console.StandardInput.WriteLine("EXIT");
            //console.WaitForExit();

            

            // uncomment the test to run - OK - I may only end up running one type ;)

           //// Test - simple run a command + simple queuing
           //var myCommands = new command();
           //System.Console.Out.WriteLine("START");
           //System.Console.Out.WriteLine("queue 1");
           //System.Threading.ThreadPool.QueueUserWorkItem (new System.Threading.WaitCallback(myCommands.thread1Command));
           //System.Console.Out.WriteLine("queue 2");
           // System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(myCommands.thread2Command));
           //System.Console.Out.WriteLine("END");

            // test - multi-queue - used to learn how to wait for the end of tasks & to test general performance
            //  use the countdownevent pattern
            var myCommands = new command();
            //var state = new object();
            System.Console.Out.WriteLine("START");
            var cde = new System.Threading.CountdownEvent(1);
            for (var i = 1; i <= 100; i++)
            {
                System.Console.Out.WriteLine("queue " + i);
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(myCommands.bogusTask),cde);
                // increment the counter
                cde.AddCount(1);
            }
            // decrement the counter - we started with one !
            //  - prevents the race condition where tasks finish before queueing is done !
            cde.Signal();
            // wait for all tasks to be complete -  the final (0) countdown event
            cde.Wait();
            System.Console.Out.WriteLine("END");
        }
    }
}
