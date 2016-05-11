using Hark.HarkPackageManager;

using System.Numerics;
using System.IO;

namespace Hark.HarkPackageManager.Library
{
    public class Dependency : IdentifiablePackage
    {
        public Dependency(
            string shortName,
            int? versionMin = null,
            BigInteger? uid = null)
        {
            this.VersionMin = versionMin;
            this.ShortName = shortName;
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
            private set;
        }
        
        public int? VersionMin
        {
            get;
            private set;
        }
    }
    
    public static partial class Extensions
    {
        public static Dependency ReadDependency(this Stream stream)
        {
            return new Dependency(
                uid : stream.ReadBigInteger(),
                shortName : stream.ReadString(),
                versionMin : stream.ReadIntNull()
            );
        }
        public static void Write(this Stream stream, Dependency dependency)
        {
            stream.Write(dependency.UID);
            stream.Write(dependency.ShortName);
            stream.Write(dependency.VersionMin);
                
            stream.Flush();
        }
    }
}