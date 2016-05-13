using Hark.HarkPackageManager;

using System.Numerics;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Library
{
    public class UserUID : UID
    {
        public UserUID(UID uid)
            : base(uid.RepositoryName, uid.Id)
        { }
    }
    
    public static partial class Extensions
    {
        public static UserUID ForUser(this UID uid)
        {
            return new UserUID(uid);
        }
    }
}