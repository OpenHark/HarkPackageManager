using Hark.HarkPackageManager;

using System.Numerics;
using System.IO;

namespace Hark.HarkPackageManager.Library
{
    public class UIDManager
    {
        private UIDManager(string repositoryName)
        {
            this.RepositoryName = repositoryName;
        }
        
        public static UIDManager Instance = new UIDManager("Default");
        
        private readonly object uidMutex = new object();
        private BigInteger suid = BigInteger.Zero;
        
        public string RepositoryName
        {
            get;
            private set;
        }
        
        public UID Reserve()
        {
            lock(uidMutex)
            {
                ++suid;
                return new UID(RepositoryName, suid);
            }
        }
        
        public void Update(UID uid)
        {
            lock(uidMutex)
            {
                if(suid < uid.Id)
                    suid = uid.Id;
            }
        }
    }
}