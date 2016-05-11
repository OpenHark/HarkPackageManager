using System.Collections.Generic;
using Hark.HarkPackageManager;

using System.Numerics;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Library
{
    using Connector = Func<IEnumerable<Stream>>;
    
    public class PendingPackage : IPackage
    {
        public PendingPackage(BigInteger uid, Connector connector)
        {
            this.Connector = connector;
            this.UID = uid;
        }
        
        private readonly Connector Connector;
        private Package package = null;
        private void Load()
        {
            if(package != null)
                return;
            
            package = Connector()
                .Peek(s => { s.Write(""); s.Flush(); })
                .Where(s => s.ReadByte() == 1)
                .Select(s => s.ReadPackage(Connector))
                .DefaultIfEmpty(null)
                .First();
                
            if(package == null)
                throw new NotFoundException();
        }
        
        public BigInteger UID
        {
            get;
            private set;
        }
        
        public string ShortName
        {
            get
            {
                Load();
                return package.ShortName;
            }
            set
            {
                Load();
                package.ShortName = value;
            }
        }
        public string Name
        {
            get
            {
                Load();
                return package.Name;
            }
            set
            {
                Load();
                package.Name = value;
            }
        }
        public string Description
        {
            get
            {
                Load();
                return package.Description;
            }
            set
            {
                Load();
                package.Description = value;
            }
        }
        public PackageState State
        {
            get
            {
                Load();
                return package.State;
            }
            set
            {
                Load();
                package.State = value;
            }
        }
        public List<IPackageVersion> Versions
        {
            get
            {
                Load();
                return package.Versions;
            }
        }
    }
    
    public static partial class Extensions
    {
        public static PendingPackage ReadPendingPackage(this Stream stream, Connector connector)
        {
            return new PendingPackage(stream.ReadBigInteger(), connector);
        }
    }
}