using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace recursive_robocopy.classes
{
    class directoryHelper
    {
        internal static string computeTargetFolder(string currDir, string SourceFolder, string TargetFolder)
        {
            return currDir.Replace(SourceFolder, TargetFolder);
        }
    }
}
