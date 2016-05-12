using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.IO;

namespace Hark.HarkPackageManager.Library
{
    public delegate IEnumerable<Stream> Connector(UID uid);
}