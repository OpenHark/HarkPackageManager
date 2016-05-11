using Hark.HarkPackageManager.Library;
using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Server
{
    public class ClientRooter
    {
        public ClientRooter(Context context)
        {
            this.DefaultClientManager = new Commands.DefaultClientManager();
            this.ClientManagers = new List<IClientManager>();
            this.Context = context;
        }
        
        public static ClientRooter CreateFromReflexion(Context context)
        {
            ClientRooter cr = new ClientRooter(context);
            
            typeof(ClientRooter).Assembly.GetTypes()
                .Where(t => t.IsDefined(typeof(ClientManagerAttribute), false))
                .Select(Activator.CreateInstance)
                .Cast<IClientManager>()
                .Select(cr.Add)
                .Close();
            
            return cr;
        }
        
        public ClientRooter Add(IClientManager clientManager)
        {
            this.ClientManagers.Add(clientManager);
            return this;
        }
        
        public ClientRooter SetDefault(IClientManager defaultClientManager)
        {
            this.DefaultClientManager = defaultClientManager;
            return this;
        }
        
        public Context Context
        {
            get;
            private set;
        }
        
        public List<IClientManager> ClientManagers
        {
            get;
            private set;
        }
        
        public IClientManager DefaultClientManager
        {
            get;
            set;
        }
        
        public bool Root(Stream stream)
        {
            try
            {
                string command = stream.ReadWord();
                command = command.Trim().ToLower();
                
                Console.WriteLine(" :: " + command);
                
                return ClientManagers
                    .Where(cm => cm.Name == command)
                    .DefaultIfEmpty(DefaultClientManager)
                    .Select(cm => cm.Execute(stream, Context))
                    .First();
            }
            catch
            {
                return false;
            }
        }
    }
}