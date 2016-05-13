using Hark.HarkPackageManager.Library;
using Hark.HarkPackageManager;

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
                this.UserAuthentication = stream.ReadUserAuthentication();
                this.ClientStream = stream;
                this.Context = context;
                this.User = Context.Users
                    .Where(u => u.Name == UserAuthentication.Name)
                    .Where(u => u.SecuredPassword == UserAuthentication.SecuredPassword)
                    .FirstOrDefault();
                
                stream.ReadTimeout = Timeout;
                
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