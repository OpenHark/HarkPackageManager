using Hark.HarkPackageManager.Library;
using Hark.HarkPackageManager;

using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Server
{
    public class Context
    {
        public Context()
        {
            this.Packages = new List<Package>();
        }
        
        public List<Package> Packages
        {
            get;
            private set;
        }
        
        public IEnumerable<Package> PackagesByName(string name)
        {
            Regex rname = name.ToWildcardRegex();
            
            return Packages
                .Where(p =>
                    rname.IsMatch(p.ShortName)
                    || rname.IsMatch(p.Name)
                    || rname.IsMatch(p.Description));
        }
        
        public bool Save(string filePath = null)
        {
            if(filePath == null)
                filePath = Starter.Settings.packageFilePath;
            
            try
            {
                using(Stream stream = File.Open(filePath, FileMode.Create))
                {
                    return Save(stream);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public bool Save(Stream stream)
        {
            try
            {
                stream.Write(Packages.Count());
                Packages.ForEach(stream.DeepWrite);
                stream.Flush();
                
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        
        public static Context Load(string filePath)
        {
            Context context = new Context();
            
            return context;
        }
        public static Context Load(Stream stream)
        {
            Context context = new Context();
            
            try
            {
                int size = stream.ReadInt();
                context.Packages = new object[size]
                    .Select(_ => stream.ReadFullPackage(null))
                    .ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            
            return context;
        }
    }
}