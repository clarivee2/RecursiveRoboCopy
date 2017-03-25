using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace recursive_robocopy.classes
{
    /// <summary>
    ///   context for recursive robocopy program 
    ///     - provide a nifty package to pass around if required
    /// </summary>
    /// <author> Christian Larivee (christian.larivee@gmail.com)</author>
    public class context
    {
        const string Command = "";

        public string[] arguments;

        protected directoryPair _folders = new directoryPair();

        public directoryPair Folders
        {
            get { return _folders; }
        }

        public context(string[] args)
        {
            directoryPair updatedFolders = consoleHelper.processParameters(args);
            _folders.SourceFolder = updatedFolders.SourceFolder;
            _folders.TargetFolder = updatedFolders.TargetFolder;
        }

        public context(string targetfolder, string sourcefolder)
        {
            this._folders.TargetFolder = targetfolder;
            this._folders.SourceFolder = sourcefolder;
        }
    }
}
