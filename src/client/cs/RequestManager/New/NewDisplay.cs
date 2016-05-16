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
        public void NewDisplay(string name)
        {
            string fileName;
            if(!NewExists(name, out fileName))
                return;
            
            using(Stream stream = File.Open(fileName, FileMode.Open))
            {
                PackageBuilder pb = stream.ReadPackageBuilder();
                
                Console.WriteLine("Information about {0} :", name.Trim());
                Console.WriteLine(" Version : {0}", pb.Version);
                Console.WriteLine(" Is stable : {0}", pb.IsStable);
                Console.WriteLine(" State : {0}", pb.State);
                Console.WriteLine(" Description : {0}", pb.Description);
                Console.WriteLine(" Repository name : {0}", pb.RepositoryName);
                
                string substrId = "   {0} :: {1}";
                
                Console.WriteLine(" File" + pb.Files.Count().ToPlural("s") + " :");
                int i = 0;
                foreach(var f in pb.Files)
                {
                    string substr2 = "      {0} : {1}";
                    string substr3 = "         {0}";
                    
                    ++i;
                    Console.WriteLine(substrId, i, "");
                    Console.WriteLine(substr2, "Description", f.Description);
                    Console.WriteLine(substr2, "Destination path", f.DestinationPath);
                    
                    Console.WriteLine(substr2, "File(s)", "");
                    f.Files.ForEach(s => Console.WriteLine(substr3, s));
                    
                    Console.WriteLine(substr2, "Folder(s)", "");
                    f.Folders.ForEach(s => Console.WriteLine(substr3, s));
                }
                
                Console.WriteLine(" Dependenc" + pb.Dependencies.Count().ToPlural("ies", "y") + " :");
                pb.Dependencies
                    .Select(d => d.Uid.ToString() + " / " + (d.VersionMin ?? 0))
                    .ForEach((s,id) => Console.WriteLine(substrId, id + 1, s));
                
                Console.WriteLine(" Access restriction" + pb.AccessRestrictions.Count().ToPlural("s") + " :");
                pb.AccessRestrictions
                    .ForEach((s,id) => Console.WriteLine(substrId, id + 1, s));
                    
                Console.WriteLine(" Owner filter" + pb.OwnerRestrictions.Count().ToPlural("s") + " :");
                pb.OwnerRestrictions
                    .ForEach((s,id) => Console.WriteLine(substrId, id + 1, s));
            }
        }
    }
}