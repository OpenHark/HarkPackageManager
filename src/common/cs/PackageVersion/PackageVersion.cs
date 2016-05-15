using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Library
{
    public class PackageVersion : IPackageVersion
    {
        public PackageVersion(
            int version,
            bool isStable,
            string description,
            PackageUID package,
            PackageVersionUID uid = null,
            List<Dependency> dependencies = null,
            List<PackageFile> files = null)
        {
            this.Dependencies = dependencies ?? new List<Dependency>();
            this.Description = description;
            this.IsStable = isStable;
            this.Version = version;
            this.PackageUid = package;
            this.Files = files ?? new List<PackageFile>();
            this.Uid = uid ?? UIDManager.Instance.Reserve().ForPackageVersion();
            
            UIDManager.Instance.Update(this.Uid);
        }
        
        public PackageVersionUID Uid
        {
            get;
            private set;
        }
        
        public int Version
        {
            get;
            set;
        }
        public bool IsStable
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public PackageUID PackageUid
        {
            get;
            internal set;
        }
        public List<Dependency> Dependencies
        {
            get;
            private set;
        }
        public List<PackageFile> Files
        {
            get;
            private set;
        }
    }
    
    public static partial class Extensions
    {
        public static void Write(this Stream stream, PackageVersion value)
        {
            stream.Write(1); // Data version (compatibility)
            
            stream.Write(value.Uid);
            stream.Write(value.PackageUid);
            stream.Write(value.Version);
            stream.Write(value.IsStable);
            stream.Write(value.Description);
            
            stream.Write(value.Dependencies.Count());
            value.Dependencies.ForEach(stream.Write);
            
            stream.Write(value.Files.Count());
            value.Files.ForEach(stream.Write);
            
            stream.Flush();
        }
        public static PackageVersion ReadPackageVersion(this Stream stream, Connector connector)
        {
            int dataVersion = stream.ReadInt();
            
            return new PackageVersion(
                uid : stream.ReadUid().ForPackageVersion(),
                package : stream.ReadUid().ForPackage(),
                version : stream.ReadInt(),
                isStable : stream.ReadBool(),
                description : stream.ReadString(),
                dependencies : new object[stream.ReadInt()]
                    .Select(_ => stream.ReadDependency())
                    .ToList(),
                files : new object[stream.ReadInt()]
                    .Select(_ => stream.ReadPackageFile())
                    .ToList()
            );
        }
    }
}