using Hark.HarkPackageManager.Library;
using Hark.HarkPackageManager;

using System.Text.RegularExpressions;
using System.Numerics;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Server.Commands
{
    [ClientManagerAttribute]
    public class List : IClientManager
    {
        public List()
            : base("list")
        { }
        
        protected override bool Execute()
        {
            ClientStream.Write(true);
            
            lock(Context)
            {
                Context.PackagesByName(ClientStream.ReadWord())
                    .Global(e => ClientStream.Write(e.Count()))
                    .ForEach(ClientStream.Write);
            }
                
            ClientStream.Flush();
            
            return false;
        }
    }
}