using Hark.HarkPackageManager.Library;
using Hark.HarkPackageManager;

using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Server
{
    public class Context
    {
        public Context()
        {
            this.Packages = new List<Package>();
        }
        
        public List<Package> Packages
        {
            get;
            private set;
        }
        
        public IEnumerable<Package> PackagesByName(string name)
        {
            Regex rname = name.ToWildcardRegex();
            
            return Packages
                .Where(p =>
                    rname.IsMatch(p.ShortName)
                    || rname.IsMatch(p.Name)
                    || rname.IsMatch(p.Description));
        }
        
        public bool Save()
        {
            // TODO : ...
            return false;
        }
        
        public static Context Load(string filePath)
        {
            Context context = new Context();
            // TODO : ...
            return context;
        }
    }
}