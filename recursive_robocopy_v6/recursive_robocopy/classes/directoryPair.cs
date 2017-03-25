using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace recursive_robocopy.classes
{
    /// <summary>
    ///   class to represent the idea of source and target folders in a robocopy/copy situation.
    /// </summary>
    public class directoryPair
    {
        private string _targetFolder;

        public string TargetFolder
        {
            get { return _targetFolder; }
            set { _targetFolder = value; }
        }
    
        private string _sourceFolder;

        public string SourceFolder
        {
            get { return _sourceFolder; }
            set { _sourceFolder = value; }
        }

        public directoryPair(string sourcefolder, string targetfolder)
        {
            this._sourceFolder = sourcefolder;
            this._targetFolder = targetfolder;
        }

        /// <summary>
        ///   copy constructor
        /// </summary>
        /// <param name="dirpair">the directory pair to copy from</param>
        public directoryPair(directoryPair dirpair)
        {
            this._sourceFolder = dirpair._sourceFolder;
            this._targetFolder = dirpair._targetFolder;
        }

        public directoryPair()
        {
            _sourceFolder = "";
            _targetFolder = "";
        }

    }
}
