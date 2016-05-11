using Hark.HarkPackageManager.Library;
using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Server
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ClientManagerAttribute : System.Attribute 
    { }
}