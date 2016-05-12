using Hark.HarkPackageManager.Library;
using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Client
{
    public partial class RequestManager
    {
        public void Version(string uid)
        {
            try
            {
                IPackageVersion pv = ConnectRepositories("version " + uid)
                    .First()
                    .ReadPackageVersion(u => ConnectRepositories(uid:u));
                    
                Console.WriteLine(pv.Uid + " " + pv.Description);
            }
            catch
            {
                Console.WriteLine("Couldn't find a package with a name matching with \"" + uid + "\"");
            }
        }
    }
}