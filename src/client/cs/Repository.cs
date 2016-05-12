using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.Net.Sockets;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Client
{
    public class Repository
    {
        public Repository(string ip, ushort port)
        {
            this.Port = port;
            this.Ip = ip;
        }
        
        public string Ip
        {
            get;
            private set;
        }
        
        public ushort Port
        {
            get;
            private set;
        }
        
        public string RepositoryName
        {
            get;
            private set;
        }
        
        public Stream Connect(int timeout = 1000)
        {
            TcpClient client = new TcpClient(Ip, Port);
            Stream stream = client.GetStream();
            stream.ReadTimeout = timeout;
            return stream;
        }
        
        public static List<Repository> Load(string filePath)
        {
            return File.ReadLines(filePath)
                .Select(s => s.Trim())
                .Where(s => s.Length > 0)
                .Select(s => s.Split(':'))
                .Where(xs => xs.Length == 2)
                .Select(xs => Tuple.Create(xs[0].Trim(), xs[1].Trim()))
                .Where(t => t.Item1.Length > 0 && t.Item2.Length > 0)
                .Select(t =>
                {
                    ushort value;
                    if(ushort.TryParse(t.Item2, out value))
                        return new Repository(t.Item1, value);
                    return null;
                }).Where(x => x != null)
                .ToList();
        }
        
        public static void Write(string filePath, IEnumerable<Repository> repos)
        {
            using(Stream stream = File.Open(filePath, FileMode.Create, FileAccess.Write))
            {
                Write(stream, repos);
                stream.Flush();
            }
        }
        public static void Write(Stream stream, IEnumerable<Repository> repos)
        {
            repos
                .Select(r => r.Ip + ":" + r.Port + "\r\n")
                .ForEach(stream.Write);
        }
    }
}