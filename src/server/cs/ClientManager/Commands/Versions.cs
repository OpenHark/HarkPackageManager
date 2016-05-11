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
    public class Versions : IClientManager
    {
        public Versions()
            : base("versions")
        { }
        
        protected override bool Execute()
        {
            try
            {
                lock(Context)
                {
                    Context.PackagesByName(ClientStream.ReadWord())
                        .Select(p => p.Versions)
                        .First()
                        .Cast<PackageVersion>()
                        .Global(e => ClientStream.Write(true))
                        .Global(e => ClientStream.Write(e.Count()))
                        .ForEach(ClientStream.Write);
                }
            }
            catch(InvalidOperationException)
            {
                ClientStream.Write(false); // First() exception
            }
                
            ClientStream.Flush();
            
            return false;
        }
    }
}