using Hark.HarkPackageManager.Library;
using Hark.HarkPackageManager;

using System.Collections.Generic;
using System.Numerics;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Server
{
    public static class Program
    {
        public static PackageVersion CreateTest(Package parent)
        {
            return new PackageVersion(
                version : 10,
                isStable : true,
                description : "Ho ho ho",
                package : parent.Uid
            );
        }
        
        public static void Main(string[] args)
        {
            AccessRestrictionBuilder.LoadDefaultBuilders();
            
            Context context = new Context();
            Package p = new Package(
                shortName : "ppm",
                name : "Programming Package Manager",
                description : "The best Programming Package Manager ever!",
                state : PackageState.Beta
            );
            User u = new User("Abcd", new byte[] { 166, 28, 37, 78, 111, 78, 95, 232, 110, 144, 172, 82, 190, 231, 205, 140, 131, 103, 178, 249, 181, 3, 16, 65, 15, 128, 36, 210, 158, 154, 172, 171 });
            p.AccessRestrictions.Add(new UserAccessRestriction(u.Uid));
            context.Packages.Add(p);
            context.Users.Add(u);
            
            p.Versions.Add(CreateTest(p));
            
            Starter.Methods.ManageClient = ClientRooter
                .CreateFromReflexion(context)
                .Root;
            Starter.EntryPoint.main(args);
        }
    }
}