using Hark.HarkPackageManager.Library;
using Hark.HarkPackageManager;

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
        
        public int Timeout
        {
            get;
            private set;
        }
        
        public bool Execute(Stream stream, Context context)
        {
            this.ClientStream = stream;
            this.Context = context;
            
            stream.ReadTimeout = Timeout;
            
            return Execute();
        }
        
        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <returns>True to keep the connection alive. False otherwise.</returns>
        protected abstract bool Execute();
    }
}