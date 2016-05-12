using Hark.HarkPackageManager;

using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Library
{
    public class GroupAccessRestriction : AccessRestriction
    {
        public GroupAccessRestriction(string regexGroupName)
        {
            this.RegexGroupName = regexGroupName.ToWildcardRegex();
        }
        
        public Regex RegexGroupName
        {
            get;
            private set;
        }
        
        public bool CanAccess(AccessRestrictionArgs args)
        {
            return args.User != null &&
                args.User.Groups
                .Select(g => g.Name)
                .Any(RegexGroupName.IsMatch);
        }
    }
}