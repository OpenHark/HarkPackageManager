using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.IO;

namespace Hark.HarkPackageManager.Library
{
    public class AccessRestrictionArgs
    {
        public AccessRestrictionArgs(
            List<User> users,
            List<UserGroup> groups,
            User user = null)
        {
            this.Groups = groups;
            this.Users = users;
            this.User = user;
        }
        
        public List<User> Users
        {
            get;
            private set;
        }
        
        public List<UserGroup> Groups
        {
            get;
            private set;
        }
        
        public User User
        {
            get;
            private set;
        }
    }
}