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
        public void NewRightAdd(string name, AccessRestriction right)
        {
            NewListAdd(
                name,
                pb => pb.AccessRestrictions,
                () => right
            );
        }
        
        public void NewRightRemove(string name, int rightIndex)
        {
            NewListRemove(name, rightIndex, pb => pb.AccessRestrictions);
        }
    }
}