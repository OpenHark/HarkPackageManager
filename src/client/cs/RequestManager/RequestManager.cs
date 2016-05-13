using Hark.HarkPackageManager.Library;
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
        
        public Connector Connector
        {
            get
            {
                return (cmd, args, uid) =>
                    ConnectRepositories(
                        cmd : cmd,
                        args : args,
                        uid : uid
                    );
            }
        }
        
        public IEnumerable<Stream> ConnectRepositories(
            string cmd,
            UserAuthentication user = null,
            UID uid = null,
            params string[] args)
        {
            user = user ?? UserAuthentication.NoUserAuthentication;
            
            return Repositories
                .Where(r => uid == null || r.Name.ToLower() == uid.RepositoryName.ToLower())
                .Select(r => r.Connect())
                .Where(s => s != null)
                .Peek(s => s.Write(cmd))
                .Peek(s => s.Write(user ?? UserAuthentication.NoUserAuthentication))
                .Peek(s => s.Write(args.Aggregate(
                    "",
                    (s1,s2) => s1 + " " + s2,
                    ss => ss.Trim()).GetBytes()))
                .Peek(s => s.Flush())
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