using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.IO;

namespace Hark.HarkPackageManager.Library
{
    public class AccessRestrictionArgs
    {
        public AccessRestrictionArgs(User user = null)
        {
            this.User = user;
        }
        
        public User User
        {
            get;
            private set;
        }
    }
}