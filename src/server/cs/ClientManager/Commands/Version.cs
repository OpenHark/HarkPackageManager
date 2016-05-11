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
    public class Version : IClientManager
    {
        public Version()
            : base("version")
        { }
        
        protected override bool Execute()
        {
            try
            {
                BigInteger uid = BigInteger.Parse(ClientStream.ReadWord());
                
                lock(Context)
                {
                    PackageVersion pv = Context.Packages
                        .SelectMany(p => p.Versions)
                        .Where(v => v.UID == uid)
                        .Cast<PackageVersion>()
                        .First(); // Throw if none found
                    
                    ClientStream.Write(true);
                    ClientStream.Write(pv);
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