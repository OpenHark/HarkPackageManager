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
        public void Version(string suid, UserAuthentication user)
        {
            try
            {
                UID uid = UID.Parse(suid);
                
                IPackageVersion pv = ConnectRepositories(
                        cmd : "version",
                        args : uid.Id.ToString(),
                        uid : uid,
                        user : user
                    )
                    .First()
                    .ReadPackageVersion(Connector);
                    
                Console.WriteLine(pv.Uid + " " + pv.Description);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Couldn't find a package with a name matching with \"" + suid + "\"");
            }
        }
    }
}