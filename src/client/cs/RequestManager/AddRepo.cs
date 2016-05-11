using Hark.HarkPackageManager.Library;
using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.Linq;
using System;

namespace Hark.HarkPackageManager.Client
{
    public partial class RequestManager
    {
        public void AddRepo(string ip, ushort port)
        {
            Repositories.Add(new Repository(ip, port));
            SaveRepositories();
        }
    }
}