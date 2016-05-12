using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Library
{
    public interface IPackage : IdentifiablePackage
    {
        string Name
        {
            get;
            set;
        }
        
        string Description
        {
            get;
            set;
        }
        
        PackageState State
        {
            get;
            set;
        }
        
        List<IPackageVersion> Versions
        {
            get;
        }
    }
}