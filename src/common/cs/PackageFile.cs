using Hark.HarkPackageManager;

using System.Numerics;
using System.IO;

namespace Hark.HarkPackageManager.Library
{
    public class PackageFile
    {
        public PackageFile(
            string description,
            string destinationPath,
            UID uid = null)
        {
            this.DestinationPath = destinationPath;
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
        
        public string DestinationPath
        {
            get;
            set;
        }
    }
    
    public static partial class Extensions
    {
        public static PackageFile ReadPackageFile(this Stream stream)
        {
            return new PackageFile(
                uid : stream.ReadUid(),
                description : stream.ReadString(),
                destinationPath : stream.ReadString()
            );
        }
        public static void Write(this Stream stream, PackageFile packageFile)
        {
            stream.Write(packageFile.UID);
            stream.Write(packageFile.Description);
            stream.Write(packageFile.DestinationPath);
                
            stream.Flush();
        }
    }
}