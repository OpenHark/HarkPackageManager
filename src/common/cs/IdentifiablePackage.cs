using System.Collections.Generic;
using Hark.HarkPackageManager;

using System.Numerics;

namespace Hark.HarkPackageManager.Library
{
    public interface IdentifiablePackage
    {
        BigInteger UID
        {
            get;
        }
        
        string ShortName
        {
            get;
        }
    }
}