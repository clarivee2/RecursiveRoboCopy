using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace recursive_robocopy.classes
{
    /// <summary>
    ///   Class to hold state info to support threading
    /// </summary>
    public class doRoboCopyState
    {
        private CountdownEvent cde; // total task countdown

        public CountdownEvent Cde
        {
            get { return cde; }
            set { cde = value; }
        }

        private CountdownEvent cdeActiveTasks; // active task countdown

        public CountdownEvent CdeActiveTasks
        {
            get { return cdeActiveTasks; }
            set { cdeActiveTasks = value; }
        }


        private bool sleepOnException = false;

        public bool SleepOnException
        {
            get { return sleepOnException; }
            set { sleepOnException = value; }
        }

        /// <summary>
        ///   output semaphore - allows control of sysout redirection
        /// </summary>
        public Semaphore semOutput { get; set; }

        public doRoboCopyState()
        {}

        public doRoboCopyState(CountdownEvent cde, CountdownEvent cdeactiveTasks , Semaphore semOutput, bool sleeponexception)
        {
            this.cde=cde;
            this.cdeActiveTasks = cdeactiveTasks;
            this.semOutput = semOutput;
            this.sleepOnException=sleeponexception;
        }

        
    }
}
