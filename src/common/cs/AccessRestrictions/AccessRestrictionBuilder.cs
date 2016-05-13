using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Hark.HarkPackageManager.Library
{
    public interface IAccessRestrictionBuilder
    {
        int TypeId
        {
            get;
        }
        
        AccessRestriction Read(Stream stream);
        void Write(Stream stream, AccessRestriction ar);
    }
    
    public class AccessRestrictionBuilder
    {
        private AccessRestrictionBuilder()
        {
            this.Builders = new List<IAccessRestrictionBuilder>();
        }
        
        public static readonly AccessRestrictionBuilder Instance = new AccessRestrictionBuilder();
        
        public List<IAccessRestrictionBuilder> Builders
        {
            get;
            private set;
        }
        
        public static void LoadDefaultBuilders()
        {
            Instance.Builders.Add(new GroupAccessRestriction.StreamBuilder());
            Instance.Builders.Add(new UserAccessRestriction.StreamBuilder());
        }
    }
    
    public static partial class Extensions
    {
        public static AccessRestriction ReadAccessRestriction(this Stream stream)
        {
            int typeId = stream.ReadInt();
            
            return AccessRestrictionBuilder.Instance
                .Builders
                .Where(b => b.TypeId == typeId)
                .Select(b => b.Read(stream))
                .FirstOrDefault();
        }
        public static void Write(this Stream stream, AccessRestriction ar)
        {
            stream.Write(ar.TypeId);
            
            AccessRestrictionBuilder.Instance
                .Builders
                .Where(b => b.TypeId == ar.TypeId)
                .First()
                .Write(stream, ar);
        }
    }
}