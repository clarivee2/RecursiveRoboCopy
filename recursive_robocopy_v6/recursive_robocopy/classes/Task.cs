using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace recursive_robocopy.classes
{
    /// <summary>
    ///  marker abstract class for the task pattern of thread pooling
    /// </summary>
    public abstract class Task: ITask
    {
        // task control settings
        public long maxQueuedTasks = ConfigDefaults.getMaxThreads();

    }
}
