using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Library
{
    public class User
    {
        public User(
            string name,
            string securedPassword,
            UserUID uid = null,
            List<GroupUID> groups = null)
        {
            this.SecuredPassword = securedPassword;
            this.Groups = groups ?? new List<GroupUID>();
            this.Name = name;
            this.Uid = uid ?? UIDManager.Instance.Reserve().ForUser();
            
            UIDManager.Instance.Update(this.Uid);
        }
        
        public UserUID Uid
        {
            get;
            private set;
        }
        
        public string Name
        {
            get;
            private set;
        }
        
        public string SecuredPassword
        {
            get;
            private set;
        }
        
        public List<GroupUID> Groups
        {
            get;
            private set;
        }
    }
    
    public static partial class Extensions
    {
        public static User ReadUser(this Stream stream)
        {
            return new User(
                uid : stream.ReadUid().ForUser(),
                name : stream.ReadString(),
                securedPassword : stream.ReadString(),
                groups : new object[stream.ReadInt()]
                    .Select(_ => stream.ReadUid().ForGroup())
                    .ToList()
            );
        }
        public static void Write(this Stream stream, User user)
        {
            stream.Write(user.Uid);
            stream.Write(user.Name);
            stream.Write(user.SecuredPassword);
            
            stream.Write(user.Groups.Count());
            user.Groups.ForEach(stream.Write);
            
            stream.Flush();
        }
    }
}