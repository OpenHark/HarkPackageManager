using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.IO;

namespace Hark.HarkPackageManager.Library
{
    public interface AccessRestriction
    {
        bool CanAccess(AccessRestrictionArgs args);
        
        int TypeId
        {
            get;
        }
    }
}