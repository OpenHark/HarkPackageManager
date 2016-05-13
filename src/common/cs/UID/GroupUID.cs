using Hark.HarkPackageManager;

using System.Numerics;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Library
{
    public class GroupUID : UID
    {
        public GroupUID(UID uid)
            : base(uid.RepositoryName, uid.Id)
        { }
    }
    
    public static partial class Extensions
    {
        public static GroupUID ForGroup(this UID uid)
        {
            return new GroupUID(uid);
        }
    }
}