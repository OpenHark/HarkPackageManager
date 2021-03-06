using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Library
{
    public class PendingPackageVersion : IPackageVersion
    {
        public PendingPackageVersion(PackageVersionUID uid, Connector connector)
        {
            this.Connector = connector;
            this.Uid = uid;
        }
        
        private readonly Connector Connector;
        private PackageVersion packageVersion = null;
        private void Load()
        {
            if(packageVersion == null)
                return;
            
            packageVersion = Connector("version", Uid.ToString(), Uid)
                .Where(s => s.ReadByte() == 1)
                .Select(s => s.ReadPackageVersion(Connector))
                .DefaultIfEmpty(null)
                .First();
                
            if(packageVersion == null)
                throw new NotFoundException();
        }
        
        public PackageVersionUID Uid
        {
            get;
            private set;
        }
        
        public int Version
        {
            get
            {
                Load();
                return packageVersion.Version;
            }
            set
            {
                Load();
                packageVersion.Version = value;
            }
        }
        
        public bool IsStable
        {
            get
            {
                Load();
                return packageVersion.IsStable;
            }
            set
            {
                Load();
                packageVersion.IsStable = value;
            }
        }
        
        public string Description
        {
            get
            {
                Load();
                return packageVersion.Description;
            }
            set
            {
                Load();
                packageVersion.Description = value;
            }
        }
        
        public PackageUID PackageUid
        {
            get
            {
                Load();
                return packageVersion.PackageUid;
            }
        }
        
        public List<Dependency> Dependencies
        {
            get
            {
                Load();
                return packageVersion.Dependencies;
            }
        }
        
        public List<PackageFile> Files
        {
            get
            {
                Load();
                return packageVersion.Files;
            }
        }
        
        public PackageFile Installer
        {
            get
            {
                Load();
                return packageVersion.Installer;
            }
        }
    }
    
    public static partial class Extensions
    {
        public static PendingPackageVersion ReadPendingPackageVersion(this Stream stream, Connector connector)
        {
            return new PendingPackageVersion(stream.ReadUid().ForPackageVersion(), connector);
        }
        public static void Write(this Stream stream, PendingPackageVersion ppv)
        {
            stream.Write(ppv.Uid);
            stream.Flush();
        }
    }
}