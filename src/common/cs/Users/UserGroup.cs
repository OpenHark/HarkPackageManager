using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.IO;

namespace Hark.HarkPackageManager.Library
{
    public class UserGroup
    {
        public UserGroup(
            string name,
            string description,
            bool isPublic,
            GroupUID uid = null,
            List<AccessRestriction> ownerRestrictions = null)
        {
            this.OwnerRestrictions = ownerRestrictions ?? new List<AccessRestriction>();
            this.Description = description;
            this.IsPublic = isPublic;
            this.Name = name;
            this.Uid = uid ?? UIDManager.Instance.Reserve().ForGroup();
            
            UIDManager.Instance.Update(this.Uid);
        }
        
        public GroupUID Uid
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
        
        public bool IsPublic
        {
            get;
            private set;
        }
        
        public List<AccessRestriction> OwnerRestrictions
        {
            get;
            private set;
        }
    }
    
    public static partial class Extensions
    {
        public static UserGroup ReadUserGroup(this Stream stream)
        {
            int dataVersion = stream.ReadInt();
            
            return new UserGroup(
                uid : stream.ReadUid().ForGroup(),
                name : stream.ReadString(),
                description : stream.ReadString(),
                isPublic : stream.ReadBool(),
                ownerRestrictions : new object[stream.ReadInt()]
                    .Select(_ => stream.ReadAccessRestriction())
                    .ToList()
            );
        }
        public static void Write(this Stream stream, UserGroup group)
        {
            stream.Write(1); // Data version (compatibility)
            
            stream.Write(group.Uid);
            stream.Write(group.Name);
            stream.Write(group.Description);
            stream.Write(group.IsPublic);
            
            stream.Write(group.OwnerRestrictions.Count());
            group.OwnerRestrictions.ForEach(stream.Write);
            
            stream.Flush();
        }
    }
}