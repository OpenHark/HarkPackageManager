using Hark.HarkPackageManager;

using System.Numerics;

namespace Hark.HarkPackageManager.Library
{
    public interface IdentifiablePackage
    {
        PackageUID Uid
        {
            get;
        }
    }
}