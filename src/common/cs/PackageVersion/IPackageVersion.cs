using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.Numerics;

namespace Hark.HarkPackageManager.Library
{
    public interface IPackageVersion
    {
        UID Uid
        {
            get;
        }
        
        int Version
        {
            get;
            set;
        }
        bool IsStable
        {
            get;
            set;
        }
        string Description
        {
            get;
            set;
        }
        UID PackageUid
        {
            get;
        }
        List<Dependency> Dependencies
        {
            get;
        }
        List<PackageFile> Files
        {
            get;
        }
    }
}