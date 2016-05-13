using Hark.HarkPackageManager.Library;
using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.Linq;
using System;

namespace Hark.HarkPackageManager.Client
{
    public partial class RequestManager
    {
        public void RemoveRepo(string name, string ip, int port)
        {
            Repositories.Remove(new Repository(name, ip, port));
            SaveRepositories();
        }
    }
}