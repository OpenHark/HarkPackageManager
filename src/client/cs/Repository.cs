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
        public Repository(string Name, string ip, int port)
        {
            this.Name = Name;
            this.Port = port;
            this.Ip = ip;
        }
        
        public string Ip
        {
            get;
            private set;
        }
        
        public int Port
        {
            get;
            private set;
        }
        
        public string Name
        {
            get;
            private set;
        }
        
        public override string ToString()
        {
            return Name + "@" + Ip + ":" + Port;
        }
        
        public Stream Connect(int timeout = 1000)
        {
            TcpClient client = new TcpClient(Ip, Port);
            Stream stream = client.GetStream();
            stream.ReadTimeout = timeout;
            return stream;
        }
        
        public override bool Equals(Object obj)
        {
            Repository r = obj as Repository; 
            if (r == null)
                return false;
            
            return Name.Equals(r.Name)
                && Port.Equals(r.Port)
                && Ip.Equals(r.Ip);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode()
                + this.Port.GetHashCode()
                + this.Ip.GetHashCode();
        }
        
        public static Repository Parse(string repo)
        {
            if(repo == null)
                throw new ArgumentNullException();
                
            try
            {
                repo = repo.Trim();
                
                int ipIndex = repo.IndexOf('@');
                int portIndex = repo.IndexOf(':');
                
                if(ipIndex == -1 || portIndex == -1)
                    throw new FormatException();
                
                return new Repository(
                    Name : repo.Substring(0, ipIndex),
                    ip : repo.Substring(ipIndex + 1, portIndex - ipIndex - 1),
                    port : int.Parse(repo.Substring(portIndex + 1))
                );
            }
            catch
            {
                throw new FormatException();
            }
        }
        public static bool TryParse(string repo, out Repository value)
        {
            try
            {
                value = Parse(repo);
                return true;
            }
            catch
            {
                value = null;
                return false;
            }
        }
        
        public static List<Repository> Load(string filePath)
        {
            return File.ReadLines(filePath)
                .Select(Parse)
                .Where(x => x != null)
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
                .Select(r => r.ToString() + "\r\n")
                .ForEach(stream.Write);
        }
    }
}