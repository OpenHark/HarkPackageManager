using Hark.HarkPackageManager;

using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO.Compression;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Client
{
    public partial class RequestManager : Starter.Methods.IRequestManager
    {
        public RequestManager()
        {
            this.Repositories = new List<Repository>();
        }
        
        public List<Repository> Repositories
        {
            get;
            private set;
        }
        
        public IEnumerable<Stream> ConnectRepositories()
        {
            return Repositories
                .Select(r => r.Connect())
                .Where(s => s != null);
        }
        public IEnumerable<Stream> ConnectRepositories(string cmdLine)
        {
            return ConnectRepositories()
                .Peek(s => s.Write(cmdLine.GetBytes()))
                .Where(s => s.ReadBool());
        }
        
        public void LoadRepositories(string filePath = null)
        {
            if(filePath == null)
                filePath = Starter.Settings.RepositoryFilePath;
                
            Repositories.AddRange(Repository.Load(filePath));
        }
        public void SaveRepositories(string filePath = null)
        {
            if(filePath == null)
                filePath = Starter.Settings.RepositoryFilePath;
                
            Repository.Write(filePath, Repositories);
        }
    }
}