using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Library
{
    public class PackageFileBuilder
    {
        public PackageFileBuilder(
            string description = null,
            string destinationPath = null,
            List<string> folders = null,
            List<string> files = null
        )
        {
            this.DestinationPath = DestinationPath;
            this.Description = description;
            this.Folders = folders ?? new List<string>();
            this.Files = files ?? new List<string>();
        }
        
        public string Description
        {
            get;
            set;
        }
        
        public string DestinationPath
        {
            get;
            set;
        }
        
        public List<string> Files
        {
            get;
            private set;
        }
        
        public List<string> Folders
        {
            get;
            private set;
        }
    }
    
    public static partial class Extensions
    {
        public static PackageFileBuilder ReadPackageFileBuilder(this Stream stream)
        {
            int dataVersion = stream.ReadInt();
            
            return new PackageFileBuilder(
                description : stream.ReadString(),
                destinationPath : stream.ReadString(),
                folders : new object[stream.ReadInt()]
                    .Select(_ => stream.ReadString())
                    .ToList(),
                files : new object[stream.ReadInt()]
                    .Select(_ => stream.ReadString())
                    .ToList()
            );
        }
        public static void Write(this Stream stream, PackageFileBuilder pkgFileBuilder)
        {
            stream.Write(1); // Data version (compatibility)
            
            stream.Write(pkgFileBuilder.Description);
            stream.Write(pkgFileBuilder.DestinationPath);
            
            stream.Write(pkgFileBuilder.Folders.Count());
            pkgFileBuilder.Folders.ForEach(stream.Write);
            
            stream.Write(pkgFileBuilder.Files.Count());
            pkgFileBuilder.Files.ForEach(stream.Write);
            
            stream.Flush();
        }
    }
}