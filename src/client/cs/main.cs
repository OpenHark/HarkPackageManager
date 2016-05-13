using Hark.HarkPackageManager.Library;
using Hark.HarkPackageManager;

using System.IO;
using System;

namespace Hark.HarkPackageManager.Client
{
    class Program
    {
        public static void Main(string[] args)
        {
            AccessRestrictionBuilder.LoadDefaultBuilders();
            
            RequestManager rm = new RequestManager();
            
            rm.LoadRepositories();
            
            Starter.Methods.RequestManager = rm;
            Starter.EntryPoint.main(args);
        }
    }
}