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
        public void List(string name, UserAuthentication user)
        {
            var result = ConnectRepositories(
                    cmd : "list",
                    args : name,
                    user : user
                )
                .Select(s =>
                {
                    return new byte[s.ReadInt()]
                        .Select(_ => s.ReadPackage(Connector));
                })
                .DefaultIfEmpty(Enumerable.Empty<Package>())
                .SelectMany(e => e)
                .ToList();
                
            int nbFound = result.Count();
            if(nbFound == 0)
            {
                Console.WriteLine("Couldn't find a package with a name matching with \"" + name + "\"");
            }
            else
            {
                Console.WriteLine("Found " + nbFound + " package" + nbFound.ToPlural() + " matching with \"" + name + "\".");
                result
                    .Select(p => p.ShortName + " :: " + p.Name + " [" + p.State + "]")
                    .ForEach(x => Console.WriteLine(x));
            }
        }
    }
}