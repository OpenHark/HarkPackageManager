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
        public void UserList(string name)
        {
            var result = ConnectRepositories(
                    cmd : "user-list",
                    args : name
                )
                .Select(s =>
                {
                    return new byte[s.ReadInt()]
                        .Select(_ => s.ReadUser());
                })
                .DefaultIfEmpty(Enumerable.Empty<User>())
                .SelectMany(e => e)
                .ToList();
                
            int nbFound = result.Count();
            if(nbFound == 0)
            {
                Console.WriteLine("Couldn't find a user with a name matching with \"" + name + "\"");
            }
            else
            {
                Console.WriteLine("Found " + nbFound + " user" + nbFound.ToPlural() + " matching with \"" + name + "\".");
                result
                    .Select(p => p.Uid + " :: " + p.Name)
                    .ForEach(x => Console.WriteLine(x));
            }
        }
    }
}