using Hark.HarkPackageManager;

using System.Numerics;
using System.IO;

namespace Hark.HarkPackageManager.Library
{
    public class UserGroup
    {
        public UserGroup(string name)
        {
            this.Name = name;
        }
        
        public string Name
        {
            get;
            private set;
        }
    }
}