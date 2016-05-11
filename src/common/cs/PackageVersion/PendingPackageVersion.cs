using System.Collections.Generic;
using Hark.HarkPackageManager;

using System.Numerics;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Library
{
    using Connector = Func<IEnumerable<Stream>>;
    
    public class PendingPackageVersion : IPackageVersion
    {
        public PendingPackageVersion(BigInteger uid, Connector connector)
        {
            this.Connector = connector;
            this.UID = uid;
        }
        
        private readonly Connector Connector;
        private PackageVersion packageVersion = null;
        private void Load()
        {
            if(packageVersion == null)
                return;
            
            packageVersion = Connector()
                .Peek(s => { s.Write("version " + UID); s.Flush(); })
                .Where(s => s.ReadByte() == 1)
                .Select(s => s.ReadPackageVersion(Connector))
                .DefaultIfEmpty(null)
                .First();
                
            if(packageVersion == null)
                throw new NotFoundException();
        }
        
        public BigInteger UID
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
        
        public IPackage Package
        {
            get
            {
                Load();
                return packageVersion.Package;
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
    }
    
    public static partial class Extensions
    {
        public static PendingPackageVersion ReadPendingPackageVersion(this Stream stream, Connector connector)
        {
            return new PendingPackageVersion(stream.ReadBigInteger(), connector);
        }
        public static void Write(this Stream stream, PendingPackageVersion ppv)
        {
            stream.Write(ppv.UID);
            stream.Flush();
        }
    }
}