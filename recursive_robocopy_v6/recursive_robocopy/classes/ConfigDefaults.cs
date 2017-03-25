using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace recursive_robocopy.classes
{
    /// <summary>
    ///   class to provide absolute defaults for config items.  
    ///   Kept things simple - configuration is read "inline" in the code.
    /// </summary>
    public class ConfigDefaults
    {
        public const long kMaxTasks = 10;

        //todo convert all these to properties !!!

        /// <summary>
        ///   get Max Task from config - if not found use the default
        /// </summary>
        /// <returns></returns>
        internal static long getMaxThreads()
        {
            // read from config item & handle the case where it doesn't exist
            long configValue = (int) Properties.Settings.Default.MaxThreads;
            
            return configValue;
        }

        /// <summary>
        ///   get Robocopy command from config - if not found use the default
        /// </summary>
        /// <returns></returns>
        internal static string getRobocopyCommand()
        {
            // read from config item & handle the case where it doesn't exist
            string configValue = Properties.Settings.Default.robocopy_command;

            return configValue;
        }

        /// <summary>
        ///   get file exclusions from config - if not found use the default
        ///     - NOTE : adds the correct switch (xf) 
        /// </summary>
        /// <returns></returns>
        internal static string getFileExclusions()
        {
            // read from config item & handle the case where it doesn't exist
            string configValue = Properties.Settings.Default.file_exclusions;

            // add the file exclusion switch if not blank
            if (!string.IsNullOrEmpty(configValue))
                configValue = ApplicationConstants.kRobocopyFileExclusionSwitch + " " + configValue;

            return configValue;
        }

        internal static int numLevels()
        {

            int configValue = Properties.Settings.Default.numLevels;

            if (configValue > 0)
            {
                return configValue;
            }
            else
                return 1; // absolute default
        }
    }
}
