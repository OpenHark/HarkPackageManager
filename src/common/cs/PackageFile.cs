using Hark.HarkPackageManager;

using System.Numerics;
using System.IO;

namespace Hark.HarkPackageManager.Library
{
    public class PackageFile
    {
        public PackageFile(
            string description,
            UID uid = null)
        {
            this.Description = description;
            this.UID = uid ?? UIDManager.Instance.Reserve();
            
            UIDManager.Instance.Update(this.UID);
        }
        
        public UID UID
        {
            get;
            private set;
        }
        
        public string Description
        {
            get;
            set;
        }
    }
    
    public static partial class Extensions
    {
        public static PackageFile ReadPackageFile(this Stream stream)
        {
            int dataVersion = stream.ReadInt();
            
            return new PackageFile(
                uid : stream.ReadUid(),
                description : stream.ReadString()
            );
        }
        public static void Write(this Stream stream, PackageFile packageFile)
        {
            stream.Write(1); // Data version (compatibility)
            
            stream.Write(packageFile.UID);
            stream.Write(packageFile.Description);
                
            stream.Flush();
        }
    }
}