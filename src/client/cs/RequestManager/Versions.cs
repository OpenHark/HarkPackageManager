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
        public void Versions(string name, UserAuthentication user)
        {
            try
            {
                Stream stream = ConnectRepositories(
                    cmd : "versions",
                    args : name,
                    user : user
                ).First();
                var result = new byte[stream.ReadInt()]
                    .Select(_ => stream.ReadPackageVersion(Connector))
                    .ToList();
                    
                int nbFound = result.Count();
                if(nbFound == 0)
                {
                    Console.WriteLine("The package found contains no version.");
                }
                else
                {
                    Console.WriteLine("Found " + nbFound + " version" + nbFound.ToPlural() + " matching the package name with \"" + name + "\".");
                    result
                        .Select(p => p.Version + " " + p.Uid)
                        .ForEach(x => Console.WriteLine(x));
                }
            }
            catch
            {
                Console.WriteLine("Couldn't find a package with a name matching with \"" + name + "\"");
            }
        }
    }
}