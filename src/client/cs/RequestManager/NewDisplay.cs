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
            string fileName = name.Trim().ToLower() + ".pkg";
            
            if(!File.Exists(fileName))
            {
                Console.Error.WriteLine("Can't find the local package \"{0}\"", fileName);
                return;
            }
            
            using(Stream stream = File.Open(fileName, FileMode.Open))
            {
                PackageBuilder pb = stream.ReadPackageBuilder();
                
                Console.WriteLine("Information about {0} :", name.Trim());
                Console.WriteLine(" Version : {0}", pb.Version);
                Console.WriteLine(" Is stable : {0}", pb.IsStable);
                Console.WriteLine(" State : {0}", pb.State);
                Console.WriteLine(" Description : {0}", pb.Description);
                Console.WriteLine(" Repository name : {0}", pb.RepositoryName);
                
                string substr = "   {0}";
                
                Console.WriteLine(" Files :");
                int i = 0;
                foreach(var f in pb.Files)
                {
                    string substr2 = "      {0} : {1}";
                    string substr3 = "         {0}";
                    
                    ++i;
                    Console.WriteLine(substr, i);
                    Console.WriteLine(substr2, "Description", f.Description);
                    Console.WriteLine(substr2, "Destination path", f.DestinationPath);
                    
                    Console.WriteLine(substr2, "Files", "");
                    f.Files.ForEach(s => Console.WriteLine(substr3, s));
                    
                    Console.WriteLine(substr2, "Folders", "");
                    f.Folders.ForEach(s => Console.WriteLine(substr3, s));
                }
                        
                Console.WriteLine(" Dependencies :");
                pb.Dependencies
                    .Select(d => d.Uid.ToString() + " / " + (d.VersionMin ?? 0))
                    .ForEach(s => Console.WriteLine(substr, s));
            }
        }
    }
}