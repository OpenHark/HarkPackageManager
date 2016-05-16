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
        public void NewEdit(
            string name,
            string repositoryName,
            string newName,
            int? version,
            bool? isStable,
            PackageState? state,
            string description)
        {
            if(newName == null &&
                repositoryName == null &&
                version == null &&
                isStable == null &&
                state == null &&
                description == null)
            {
                Console.WriteLine("Nothing to do.");
                return;
            }
            
            string fileName;
            if(!NewExists(name, out fileName))
                return;
            
            PackageBuilder pb;
            using(Stream stream = File.Open(fileName, FileMode.Open))
            {
                pb = stream.ReadPackageBuilder();
                
                pb.PackageName = newName ?? pb.PackageName;
                pb.Version = version ?? pb.Version;
                pb.IsStable = isStable ?? pb.IsStable;
                pb.State = state ?? pb.State;
                pb.Description = description ?? pb.Description;
                pb.RepositoryName = repositoryName ?? pb.RepositoryName;
            }
            
            using(Stream stream = File.Open(fileName, FileMode.Truncate))
                stream.Write(pb);
                
            Console.WriteLine("Pacakge {0} updated.", fileName);
        }
    }
}