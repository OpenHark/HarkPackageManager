using Hark.HarkPackageManager;

using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Library
{
    public class UserAccessRestriction : AccessRestriction
    {
        public UserAccessRestriction(string regexUserName)
        {
            this.RegexUserName = regexUserName.ToWildcardRegex();
        }
        
        public Regex RegexUserName
        {
            get;
            private set;
        }
        
        public bool CanAccess(AccessRestrictionArgs args)
        {
            return args.User != null && RegexUserName.IsMatch(args.User.Name);
        }
    }
}