using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Library
{
    public class PackageBuilder
    {
        public PackageBuilder(
            string packageName,
            int version,
            bool isStable = true,
            string repositoryName = null,
            PackageState state = PackageState.Release,
            string description = null,
            string installer = null,
            List<Dependency> dependencies = null,
            List<PackageFileBuilder> files = null,
            List<AccessRestriction> ownerRestrictions = null,
            List<AccessRestriction> accessRestrictions = null
        )
        {
            this.AccessRestrictions = accessRestrictions ?? new List<AccessRestriction>();
            this.OwnerRestrictions = ownerRestrictions ?? new List<AccessRestriction>();
            this.Dependencies = dependencies ?? new List<Dependency>();
            this.Files = files ?? new List<PackageFileBuilder>();
            
            this.RepositoryName = repositoryName ?? "";
            this.PackageName = packageName;
            this.Description = description ?? "";
            this.Installer = installer ?? "";
            this.IsStable = isStable;
            this.Version = version;
            this.State = state;
        }
        
        public string RepositoryName
        {
            get;
            set;
        }
        
        public string PackageName
        {
            get;
            set;
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
        
        public PackageState State
        {
            get;
            set;
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
        
        public List<Dependency> Dependencies
        {
            get;
            private set;
        }
        
        public List<PackageFileBuilder> Files
        {
            get;
            private set;
        }
        
        private string installer;
        public string Installer
        {
            get
            {
                return installer;
            }
            set
            {
                installer = value ?? "";
            }
        }
        
        public void Produce(
            string destinationPath = null,
            bool overrideExisting = true)
        {
            destinationPath = destinationPath ?? Path.GetTempFileName();
            
            // TODO : ...
        }
        
        public void Push(string repositoryName = null)
        {
            repositoryName = repositoryName ?? RepositoryName;
            
            if(String.IsNullOrEmpty(repositoryName))
                throw new Exception("Repository name not defined.");
            
            // TODO : ...
        }
    }
    
    public static partial class Extensions
    {
        public static PackageBuilder ReadPackageBuilder(this Stream stream)
        {
            int dataVersion = stream.ReadInt();
            
            return new PackageBuilder(
                repositoryName : stream.ReadString(),
                packageName : stream.ReadString(),
                version : stream.ReadInt(),
                isStable : stream.ReadBool(),
                state : stream.ReadPackageState(),
                description : stream.ReadString(),
                installer : stream.ReadString(),
                accessRestrictions : new object[stream.ReadInt()]
                    .Select(_ => stream.ReadAccessRestriction())
                    .ToList(),
                ownerRestrictions : new object[stream.ReadInt()]
                    .Select(_ => stream.ReadAccessRestriction())
                    .ToList(),
                dependencies : new object[stream.ReadInt()]
                    .Select(_ => stream.ReadDependency())
                    .ToList(),
                files : new object[stream.ReadInt()]
                    .Select(_ => stream.ReadPackageFileBuilder())
                    .ToList()
            );
        }
        public static void Write(this Stream stream, PackageBuilder pkgBuilder)
        {
            stream.Write(1); // Data version (compatibility)
            
            stream.Write(pkgBuilder.RepositoryName);
            stream.Write(pkgBuilder.PackageName);
            stream.Write(pkgBuilder.Version);
            stream.Write(pkgBuilder.IsStable);
            stream.Write(pkgBuilder.State);
            stream.Write(pkgBuilder.Description);
            stream.Write(pkgBuilder.Installer);
            
            stream.Write(pkgBuilder.AccessRestrictions.Count());
            pkgBuilder.AccessRestrictions.ForEach(stream.Write);
            
            stream.Write(pkgBuilder.OwnerRestrictions.Count());
            pkgBuilder.OwnerRestrictions.ForEach(stream.Write);
            
            stream.Write(pkgBuilder.Dependencies.Count());
            pkgBuilder.Dependencies.ForEach(stream.Write);
            
            stream.Write(pkgBuilder.Files.Count());
            pkgBuilder.Files.ForEach(stream.Write);
            
            stream.Flush();
        }
    }
}