using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.Numerics;

namespace Hark.HarkPackageManager.Library
{
    public interface IPackageVersion
    {
        PackageVersionUID Uid
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
        
        PackageUID PackageUid
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
        
        PackageFile Installer
        {
            get;
        }
    }
}