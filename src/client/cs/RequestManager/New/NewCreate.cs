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
        public void NewCreate(string name, int version)
        {
            name = name.Trim();
            
            if(!new Regex("[a-zA-Z0-9_\\-]+").IsMatch(name))
            {
                Console.Error.WriteLine("Invalid package name {0}.", name);
                return;
            }
            
            string fileName;
            if(!NewExists(name, out fileName, false))
                return;
            
            using(Stream stream = File.Open(fileName, FileMode.CreateNew))
            {
                stream.Write(new PackageBuilder(name, version));
            }
                
            Console.WriteLine("Package {0} created.", fileName);
        }
    }
}