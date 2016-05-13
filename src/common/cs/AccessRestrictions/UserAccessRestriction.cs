using Hark.HarkPackageManager;

using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Library
{
    public class UserAccessRestriction : AccessRestriction
    {
        public UserAccessRestriction(UID userUid)
        {
            this.UserUid = userUid;
        }
        
        public UID UserUid
        {
            get;
            private set;
        }
        
        public const int TypeUId = 2;
        public int TypeId
        {
            get { return UserAccessRestriction.TypeUId; }
        }
        
        public bool CanAccess(AccessRestrictionArgs args)
        {
            return args.User != null && args.User.Uid == UserUid;
        }
        
        public class StreamBuilder : IAccessRestrictionBuilder
        {
            public int TypeId
            {
                get { return UserAccessRestriction.TypeUId; }
            }
            
            public AccessRestriction Read(Stream stream)
            {
                return new UserAccessRestriction(
                    userUid : stream.ReadUid()
                );
            }
            
            public void Write(Stream stream, AccessRestriction ar)
            {
                stream.Write(((UserAccessRestriction)ar).UserUid);
            }
        }
    }
}