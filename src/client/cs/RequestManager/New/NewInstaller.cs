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
        public void NewInstaller(
            string name,
            string installerPath)
        {
            string fileName;
            if(!NewExists(name, out fileName))
                return;
            
            PackageBuilder pb;
            using(Stream stream = File.Open(fileName, FileMode.Open))
            {
                pb = stream.ReadPackageBuilder();
                
                pb.Installer = installerPath;
            }
            
            using(Stream stream = File.Open(fileName, FileMode.Truncate))
                stream.Write(pb);
                
            Console.WriteLine("Package {0} updated.", fileName);
        }
    }
}