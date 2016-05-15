using Hark.HarkPackageManager;

using System.Security.Cryptography;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Library
{
    public class UserAuthentication
    {
        public UserAuthentication(
            string name,
            byte[] password,
            bool isClearPassword = true)
        {
            if(isClearPassword)
            {
                SHA256 hasher = SHA256Managed.Create();
                this.SecurePasswordLevel1 = hasher.ComputeHash(password);
            }
            else
                this.SecurePasswordLevel1 = password;
                
            this.Name = name;
            
            this.IsNoUser = false;
        }
        public UserAuthentication()
        {
            this.SecurePasswordLevel1 = null;
            this.Name = null;
            
            this.IsNoUser = true;
        }
        
        public static readonly UserAuthentication NoUserAuthentication = new UserAuthentication();
        
        public string Name
        {
            get;
            private set;
        }
        
        public byte[] SecurePasswordLevel1
        {
            get;
            private set;
        }
        
        public bool IsNoUser
        {
            get;
            private set;
        }
        
        public override string ToString()
        {
            return Name + "@" + Convert.ToBase64String(SecurePasswordLevel1);
        }
        
        public static UserAuthentication Parse(string value)
        {
            if(value == null)
                throw new ArgumentNullException();
                
            try
            {
                string[] parts = value.Split('@');
                
                if(parts.Length != 2)
                    throw new FormatException();
                    
                parts[0] = parts[0].Trim();
                parts[1] = parts[1].Trim();
                
                if(parts[0].Length == 0 || parts[1].Length == 0)
                    throw new FormatException();
                    
                return new UserAuthentication(
                    parts[0],
                    Convert.FromBase64String(parts[1]),
                    false);
            }
            catch
            {
                throw new FormatException();
            }
        }
        public static bool TryParse(string uid, out UserAuthentication value)
        {
            try
            {
                value = Parse(uid);
                return true;
            }
            catch
            {
                value = null;
                return false;
            }
        }
    }
    
    public static partial class Extensions
    {
        public static UserAuthentication ReadUserAuthentication(this Stream stream)
        {
            int dataVersion = stream.ReadInt();
            
            if(stream.ReadBool())
                return new UserAuthentication();
            else
                return new UserAuthentication(
                    name : stream.ReadString(),
                    password : stream.ReadWrapped(),
                    isClearPassword : false
                );
        }
        public static void Write(this Stream stream, UserAuthentication ua)
        {
            stream.Write(1); // Data version (compatibility)
            
            stream.Write(ua.IsNoUser);
            
            if(!ua.IsNoUser)
            {
                stream.Write(ua.Name);
                stream.WriteWrapped(ua.SecurePasswordLevel1);
            }
            
            stream.Flush();
        }
    }
}