using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using recursive_robocopy.classes;

namespace recursive_robocopytests
{
    public class command
    {
        public void thread1Command(object state)
        {
            consoleHelper.ExecuteCommand("dir /b c:\\");
        }

        public void thread2Command(object state)
        {
            consoleHelper.ExecuteCommand("dir /b/s c:\\");
        }

        public void bogusTask(object state)
        {
            var delay = new TimeSpan(0, 0, 0, 0, new Random().Next(55000, 60000));
            System.Console.Out.WriteLine(string.Format("  START - Bogus Task - Delay - {0}", delay.ToString()));
            System.Threading.Thread.Sleep(delay);

            // decrement the count passed in
            CountdownEvent cde = (CountdownEvent)state;
            cde.Signal();
            System.Console.Out.WriteLine(string.Format("  END - Bogus Task - Delay - {0}", delay.ToString()));
        }
    }
}
