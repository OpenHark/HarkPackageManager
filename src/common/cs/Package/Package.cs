using System.Collections.Generic;
using Hark.HarkPackageManager;

using System.Numerics;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Library
{
    using Connector = Func<IEnumerable<Stream>>;
    
    public class Package : IPackage
    {
        public Package(
            string shortName,
            string name,
            string description = "",
            BigInteger? uid = null,
            PackageState state = PackageState.Release,
            List<IPackageVersion> versions = null)
        {
            this.Description = description;
            this.ShortName = shortName;
            this.Versions = versions ?? new List<IPackageVersion>();
            this.State = state;
            this.Name = name;
            this.UID = uid ?? UIDManager.Reserve();
            
            UIDManager.Update(this.UID);
        }
        
        public BigInteger UID
        {
            get;
            private set;
        }
        
        public string ShortName
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public PackageState State
        {
            get;
            set;
        }
        public List<IPackageVersion> Versions
        {
            get;
            private set;
        }
    }
    
    public static partial class Extensions
    {
        public static Package ReadPackage(this Stream stream, Connector connector)
        {
            return new Package(
                uid : stream.ReadBigInteger(),
                shortName : stream.ReadString(),
                name : stream.ReadString(),
                description : stream.ReadString(),
                state : stream.ReadPackageState(),
                versions : new object[stream.ReadInt()]
                    .Select(_ => stream.ReadPendingPackageVersion(connector))
                    .Cast<IPackageVersion>()
                    .ToList()
            );
        }
        public static void Write(this Stream stream, Package package)
        {
            stream.Write(package.UID);
            stream.Write(package.ShortName);
            stream.Write(package.Name);
            stream.Write(package.Description);
            stream.Write(package.State);
            
            stream.Write(package.Versions.Count());
            package
                .Versions
                .Select(v => v.UID)
                .ForEach(stream.Write);
                
            stream.Flush();
        }
    }
}