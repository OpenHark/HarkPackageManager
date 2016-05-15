using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.Security.Cryptography;
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
            byte[] securePassword,
            bool isSPLevel1 = true,
            UserUID uid = null,
            List<GroupUID> groups = null)
        {
            if(isSPLevel1)
            {
                SHA256 hasher = SHA256Managed.Create();
                this.SecurePasswordLevel2 = hasher.ComputeHash(securePassword);
            }
            else
                this.SecurePasswordLevel2 = securePassword;
                
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
        
        public byte[] SecurePasswordLevel2
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
            int dataVersion = stream.ReadInt();
            
            return new User(
                uid : stream.ReadUid().ForUser(),
                name : stream.ReadString(),
                isSPLevel1 : false,
                securePassword : stream.ReadWrapped(),
                groups : new object[stream.ReadInt()]
                    .Select(_ => stream.ReadUid().ForGroup())
                    .ToList()
            );
        }
        public static void Write(this Stream stream, User user)
        {
            stream.Write(1); // Data version (compatibility)
            
            stream.Write(user.Uid);
            stream.Write(user.Name);
            stream.WriteWrapped(user.SecurePasswordLevel2);
            
            stream.Write(user.Groups.Count());
            user.Groups.ForEach(stream.Write);
            
            stream.Flush();
        }
    }
}