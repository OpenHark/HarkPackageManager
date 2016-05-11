using Hark.HarkPackageManager;

using System.Numerics;
using System.IO;

namespace Hark.HarkPackageManager.Library
{
    public static class UIDManager
    {
        private static readonly object uidMutex = new object();
        private static BigInteger suid = BigInteger.Zero;
        
        public static BigInteger Reserve()
        {
            lock(uidMutex)
            {
                ++suid;
                return suid;
            }
        }
        
        public static void Update(BigInteger uid)
        {
            lock(uidMutex)
            {
                if(suid < uid)
                    suid = uid;
            }
        }
    }
}