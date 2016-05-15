using Hark.HarkPackageManager;

using System.Numerics;
using System.IO;

namespace Hark.HarkPackageManager.Library
{
    public class Dependency : IdentifiablePackage
    {
        public Dependency(
            int? versionMin = null,
            PackageUID uid = null)
        {
            this.VersionMin = versionMin;
            this.Uid = uid ?? UIDManager.Instance.Reserve().ForPackage();
            
            UIDManager.Instance.Update(this.Uid);
        }
        
        public PackageUID Uid
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
            int dataVersion = stream.ReadInt();
            
            return new Dependency(
                uid : stream.ReadUid().ForPackage(),
                versionMin : stream.ReadIntNull()
            );
        }
        public static void Write(this Stream stream, Dependency dependency)
        {
            stream.Write(1); // Data version (compatibility)
            
            stream.Write(dependency.Uid);
            stream.Write(dependency.VersionMin);
                
            stream.Flush();
        }
    }
}