using Hark.HarkPackageManager.Library;
using Hark.HarkPackageManager;

using System.IO;
using System;

namespace Hark.HarkPackageManager.Server.Commands
{
    public class DefaultClientManager : IClientManager
    {
        public DefaultClientManager()
            : base("")
        { }
        
        protected override bool Execute()
        {
            Console.WriteLine("Default!");
            ClientStream.Write(false);
            return false;
        }
    }
}