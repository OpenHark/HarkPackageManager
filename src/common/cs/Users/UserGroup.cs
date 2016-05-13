using Hark.HarkPackageManager;

using System.Numerics;
using System.IO;

namespace Hark.HarkPackageManager.Library
{
    public class UserGroup
    {
        public UserGroup(string name, string description, UID uid = null)
        {
            this.Description = description;
            this.Name = name;
            this.Uid = uid ?? UIDManager.Instance.Reserve();
            
            UIDManager.Instance.Update(this.Uid);
        }
        
        public UID Uid
        {
            get;
            private set;
        }
        
        public string Name
        {
            get;
            private set;
        }
        
        public string Description
        {
            get;
            private set;
        }
    }
}