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
            PackageUID uid = null)
        {
            this.VersionMin = versionMin;
            this.ShortName = shortName;
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
                uid : stream.ReadUid().ForPackage(),
                shortName : stream.ReadString(),
                versionMin : stream.ReadIntNull()
            );
        }
        public static void Write(this Stream stream, Dependency dependency)
        {
            stream.Write(dependency.Uid);
            stream.Write(dependency.ShortName);
            stream.Write(dependency.VersionMin);
                
            stream.Flush();
        }
    }
}