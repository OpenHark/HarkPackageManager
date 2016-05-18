using Hark.HarkPackageManager.Library;
using Hark.HarkPackageManager;

using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Client
{
    public partial class RequestManager
    {
        public void NewFileAdd(
            string name,
            List<string> files,
            List<string> folders,
            string description)
        {
            NewListAdd(
                name,
                pb => pb.Files,
                () => new PackageFileBuilder(
                    description : description,
                    folders : folders,
                    files : files
                )
            );
        }
        
        public void NewFileRemove(string name, int fileIndex)
        {
            NewListRemove(name, fileIndex, pb => pb.Files);
        }
    }
}