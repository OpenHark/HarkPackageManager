using Hark.HarkPackageManager.Library;
using Hark.HarkPackageManager;

using System.Security.Cryptography;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Server
{
    public abstract class IClientManager
    {
        public IClientManager(string name, int timeout = 50)
        {
            this.Name = name.Trim().ToLower();
            this.Timeout = timeout;
        }
        
        public string Name
        {
            get;
            private set;
        }
        
        protected Stream ClientStream
        {
            get;
            private set;
        }
        
        protected Context Context
        {
            get;
            private set;
        }
        
        public User User
        {
            get;
            private set;
        }
        
        public UserAuthentication UserAuthentication
        {
            get;
            private set;
        }
        
        public int Timeout
        {
            get;
            private set;
        }
        
        public AccessRestrictionArgs CreateAccessestrictionArgs()
        {
            return new AccessRestrictionArgs(
                users : Context.Users,
                groups : Context.Groups,
                user : User
            );
        }
        
        public bool Execute(Stream stream, Context context)
        {
            lock(this)
            {
                stream.ReadTimeout = Timeout;
                
                this.UserAuthentication = stream.ReadUserAuthentication();
                this.ClientStream = stream;
                this.Context = context;
                
                if(this.UserAuthentication.IsNoUser)
                    this.User = null;
                else
                {
                    SHA256 hasher = SHA256Managed.Create();
                    byte[] securePasswordLevel2 = hasher.ComputeHash(this.UserAuthentication.SecurePasswordLevel1);

                    this.User = securePasswordLevel2 == null ? null : Context.Users
                        .Where(u => u.Name.ToLower() == UserAuthentication.Name.ToLower())
                        .Where(u => Enumerable.SequenceEqual(u.SecurePasswordLevel2, securePasswordLevel2))
                        .FirstOrDefault();
                }
                
                return Execute();
            }
        }
        
        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <returns>True to keep the connection alive. False otherwise.</returns>
        protected abstract bool Execute();
    }
}