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
        public GroupAccessRestriction(UID groupUid)
        {
            this.GroupUid = groupUid;
        }
        
        public UID GroupUid
        {
            get;
            private set;
        }
        
        public const int TypeUId = 1;
        public int TypeId
        {
            get { return GroupAccessRestriction.TypeUId; }
        }
        
        public bool CanAccess(AccessRestrictionArgs args)
        {
            return args.User != null &&
                args.User.Groups.Any(g => g == GroupUid);
        }
        
        public class StreamBuilder : IAccessRestrictionBuilder
        {
            public int TypeId
            {
                get { return GroupAccessRestriction.TypeUId; }
            }
            
            public AccessRestriction Read(Stream stream)
            {
                return new GroupAccessRestriction(
                    groupUid : stream.ReadUid()
                );
            }
            
            public void Write(Stream stream, AccessRestriction ar)
            {
                stream.Write(((GroupAccessRestriction)ar).GroupUid);
            }
        }
    }
}