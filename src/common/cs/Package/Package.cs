using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Library
{
    public class Package : IPackage
    {
        public Package(
            string shortName,
            string name,
            string description = "",
            GroupUID groupOwner = null,
            PackageUID uid = null,
            PackageState state = PackageState.Release,
            List<IPackageVersion> versions = null,
            List<AccessRestriction> accessRestrictions = null,
            List<AccessRestriction> ownerRestrictions = null)
        {
            this.AccessRestrictions = accessRestrictions ?? new List<AccessRestriction>();
            this.OwnerRestrictions = ownerRestrictions ?? new List<AccessRestriction>();
            this.Description = description;
            this.ShortName = shortName;
            this.Versions = versions ?? new List<IPackageVersion>();
            this.State = state;
            this.Name = name;
            this.Uid = uid ?? UIDManager.Instance.Reserve().ForPackage();
            
            UIDManager.Instance.Update(this.Uid);
        }
        
        public PackageUID Uid
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
        public List<AccessRestriction> AccessRestrictions
        {
            get;
            private set;
        }
        public List<AccessRestriction> OwnerRestrictions
        {
            get;
            private set;
        }
        
        public bool IsAuthorized(AccessRestrictionArgs args)
        {
            return AccessRestrictions.Count == 0 ||
                AccessRestrictions
                .Any(a => a.CanAccess(args));
        }
    }
    
    public static partial class Extensions
    {
        public static Package ReadPackage(this Stream stream, Connector connector)
        {
            int dataVersion = stream.ReadInt();
            
            return new Package(
                uid : stream.ReadUid().ForPackage(),
                shortName : stream.ReadString(),
                name : stream.ReadString(),
                description : stream.ReadString(),
                state : stream.ReadPackageState(),
                versions : new object[stream.ReadInt()]
                    .Select(_ => stream.ReadPendingPackageVersion(connector))
                    .Cast<IPackageVersion>()
                    .ToList(),
                accessRestrictions : new object[stream.ReadInt()]
                    .Select(_ => stream.ReadAccessRestriction())
                    .ToList(),
                ownerRestrictions : new object[stream.ReadInt()]
                    .Select(_ => stream.ReadAccessRestriction())
                    .ToList()
            );
        }
        public static void Write(this Stream stream, Package package)
        {
            stream.Write(1); // Data version (compatibility)
            
            stream.Write(package.Uid);
            stream.Write(package.ShortName);
            stream.Write(package.Name);
            stream.Write(package.Description);
            stream.Write(package.State);
            
            stream.Write(package.Versions.Count());
            package
                .Versions
                .Select(v => v.Uid)
                .ForEach(stream.Write);
                
            stream.Write(package.AccessRestrictions.Count());
            package.AccessRestrictions.ForEach(stream.Write);
                
            stream.Write(package.OwnerRestrictions.Count());
            package.OwnerRestrictions.ForEach(stream.Write);
                
            stream.Flush();
        }
        
        public static Package ReadDeepPackage(this Stream stream, Connector connector)
        {
            int dataVersion = stream.ReadInt();
            
            Package p = new Package(
                uid : stream.ReadUid().ForPackage(),
                shortName : stream.ReadString(),
                name : stream.ReadString(),
                description : stream.ReadString(),
                state : stream.ReadPackageState(),
                versions : new object[stream.ReadInt()]
                    .Select(_ => stream.ReadPackageVersion(connector))
                    .Cast<IPackageVersion>()
                    .ToList(),
                accessRestrictions : new object[stream.ReadInt()]
                    .Select(_ => stream.ReadAccessRestriction())
                    .ToList(),
                ownerRestrictions : new object[stream.ReadInt()]
                    .Select(_ => stream.ReadAccessRestriction())
                    .ToList()
            );
            
            return p;
        }
        public static void DeepWrite(this Stream stream, Package package)
        {
            stream.Write(1); // Data version (compatibility)
            
            stream.Write(package.Uid);
            stream.Write(package.ShortName);
            stream.Write(package.Name);
            stream.Write(package.Description);
            stream.Write(package.State);
            
            stream.Write(package.Versions.Count());
            package
                .Versions
                .Cast<PackageVersion>()
                .ForEach(stream.Write);
                
            stream.Write(package.AccessRestrictions.Count());
            package.AccessRestrictions.ForEach(stream.Write);
                
            stream.Write(package.OwnerRestrictions.Count());
            package.OwnerRestrictions.ForEach(stream.Write);
                
            stream.Flush();
        }
    }
}