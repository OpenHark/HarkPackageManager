using Hark.HarkPackageManager;

using System.Numerics;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Library
{
    public class UID
    {
        public UID(string repositoryName, BigInteger id)
        {
            this.RepositoryName = repositoryName.Trim();
            this.Id = id;
        }
        
        public BigInteger Id
        {
            get;
            private set;
        }
        
        public string RepositoryName
        {
            get;
            private set;
        }
        
        public override string ToString()
        {
            return Id + "@" + RepositoryName;
        }
        
        public override bool Equals(object obj)
        {
            UID uid = obj as UID;
            if(uid == null)
                return false;
            return Id == uid.Id && RepositoryName == uid.RepositoryName;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() + RepositoryName.GetHashCode();
        }
        
        public static UID Parse(string uid)
        {
            if(uid == null)
                throw new ArgumentNullException();
                
            try
            {
                string[] parts = uid.Split('@');
                if(parts.Length != 2)
                    throw new FormatException();
                return new UID(parts[1], BigInteger.Parse(parts[0]));
            }
            catch
            {
                throw new FormatException();
            }
        }
        public static bool TryParse(string uid, out UID value)
        {
            try
            {
                value = Parse(uid);
                return true;
            }
            catch
            {
                value = null;
                return false;
            }
        }
    }
    
    public static partial class Extensions
    {
        public static UID ReadUid(this Stream stream)
        {
            return new UID(
                repositoryName : stream.ReadString(),
                id : stream.ReadBigInteger()
            );
        }
        public static void Write(this Stream stream, UID uid)
        {
            stream.Write(uid.RepositoryName);
            stream.Write(uid.Id);
                
            stream.Flush();
        }
    }
}