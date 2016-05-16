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
    public class UserList : IClientManager
    {
        public UserList()
            : base("user-list")
        { }
        
        protected override bool Execute()
        {
            ClientStream.Write(true);
            
            Regex regex = ClientStream.ReadWord().ToWildcardRegex();
            
            lock(Context)
            {
                Context.Users
                    .Where(u => regex.IsMatch(u.Name))
                    .Global(e => ClientStream.Write(e.Count()))
                    .ForEach(ClientStream.Write);
            }
                
            ClientStream.Flush();
            
            return false;
        }
    }
}