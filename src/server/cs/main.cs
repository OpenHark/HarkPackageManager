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
                uid : new BigInteger(123456),
                version : 10,
                isStable : true,
                description : "Ho ho ho",
                package : parent
            );
        }
        
        public static void Main(string[] args)
        {
            Context context = new Context();
            Package p = new Package(
                shortName : "ppm",
                name : "Programming Package Manager",
                description : "The best Programming Package Manager ever!",
                state : PackageState.Beta
            );
            context.Packages.Add(p);
            
            p.Versions.Add(CreateTest(p));
            
            Starter.Methods.ManageClient = ClientRooter
                .CreateFromReflexion(context)
                .Root;
            Starter.EntryPoint.main(args);
        }
    }
}