using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.Numerics;
using System.IO;

namespace Hark.HarkPackageManager.Library
{
    public class User
    {
        public User(string name)
        {
            this.Name = name;
            this.Groups = new List<UserGroup>();
        }
        
        public string Name
        {
            get;
            private set;
        }
        
        public List<UserGroup> Groups
        {
            get;
            private set;
        }
    }
}