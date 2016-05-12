using Hark.HarkPackageManager;

using System.Numerics;
using System.IO;

namespace Hark.HarkPackageManager.Library
{
    public class UID
    {
        public UID(string repositoryName, BigInteger id)
        {
            this.RepositoryName = repositoryName.Trim();
            this.Id = id;
        }
        
        public BigInteger Id
        {
            get;
            private set;
        }
        
        public string RepositoryName
        {
            get;
            private set;
        }
        
        public override string ToString()
        {
            return Id + "@" + RepositoryName;
        }
    }
    
    public static partial class Extensions
    {
        public static UID ReadUid(this Stream stream)
        {
            return new UID(
                repositoryName : stream.ReadString(),
                id : stream.ReadBigInteger()
            );
        }
        public static void Write(this Stream stream, UID uid)
        {
            stream.Write(uid.RepositoryName);
            stream.Write(uid.Id);
                
            stream.Flush();
        }
    }
}