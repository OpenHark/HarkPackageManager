using Hark.HarkPackageManager;

using System.IO;
using System;

namespace Hark.HarkPackageManager.Library
{
    public class UserAuthentication
    {
        public UserAuthentication(string name, string clearPassword)
        {
            this.SecuredPassword = clearPassword;
            this.Name = name;
            
            this.IsNoUser = false;
        }
        public UserAuthentication()
        {
            this.SecuredPassword = null;
            this.Name = null;
            
            this.IsNoUser = true;
        }
        
        public static readonly UserAuthentication NoUserAuthentication = new UserAuthentication();
        
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
        
        public bool IsNoUser
        {
            get;
            private set;
        }
        
        public override string ToString()
        {
            return Name + "@" + SecuredPassword;
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
                return new UserAuthentication(parts[0], parts[1]);
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
            if(stream.ReadBool())
                return new UserAuthentication();
            else
                return new UserAuthentication(
                    name : stream.ReadString(),
                    clearPassword : stream.ReadString()
                );
        }
        public static void Write(this Stream stream, UserAuthentication ua)
        {
            stream.Write(ua.IsNoUser);
            
            if(!ua.IsNoUser)
            {
                stream.Write(ua.Name);
                stream.Write(ua.SecuredPassword);
            }
            
            stream.Flush();
        }
    }
}