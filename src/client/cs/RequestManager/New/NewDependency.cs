using Hark.HarkPackageManager.Library;
using Hark.HarkPackageManager;

using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Client
{
    public partial class RequestManager
    {
        public void NewDependencyAdd(string name, Dependency dep)
        {
            NewListAdd(
                name,
                pb => pb.Dependencies,
                () => dep
            );
        }
        
        public void NewDependencyRemove(string name, int depIndex)
        {
            NewListRemove(name, depIndex, pb => pb.Dependencies);
        }
    }
}