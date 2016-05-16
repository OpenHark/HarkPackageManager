using Hark.HarkPackageManager.Library;
using Hark.HarkPackageManager;

using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Client
{
    public partial class RequestManager
    {
        protected bool NewExists(string name, out string fileName, bool mustExists = true)
        {
            fileName = name.Trim().ToLower() + ".pkg";
            
            bool exists = File.Exists(fileName);
            
            if(!exists && mustExists)
            {
                Console.Error.WriteLine("Can't find the local package \"{0}\".", fileName);
                return false;
            }
            
            if(exists && !mustExists)
            {
                Console.Error.WriteLine("The local package \"{0}\" exists.", fileName);
                return false;
            }
            
            return true;
        }
        
        
        protected void NewListAdd<T>(string name, Func<PackageBuilder, IList<T>> listRefProvider, Func<T> provider)
        {
            string fileName;
            if(!NewExists(name, out fileName))
                return;
            
            PackageBuilder pb;
            using(Stream stream = File.Open(fileName, FileMode.Open))
            {
                pb = stream.ReadPackageBuilder();
                
                listRefProvider(pb).Add(provider());
            }
            
            using(Stream stream = File.Open(fileName, FileMode.Truncate))
                stream.Write(pb);
                
            Console.WriteLine("Package {0} updated.", fileName);
        }
        protected void NewListRemove(string name, int index, Func<PackageBuilder, IList> listRefProvider)
        {
            string fileName;
            if(!NewExists(name, out fileName))
                return;
            
            PackageBuilder pb;
            using(Stream stream = File.Open(fileName, FileMode.Open))
            {
                pb = stream.ReadPackageBuilder();
                IList list = listRefProvider(pb);
                
                if(index >= list.Count)
                { // Can't find filePath
                    Console.Error.WriteLine("Index \"{0}\" out of bound in {1}.", index, name);
                    Console.WriteLine("Nothing done.");
                    return;
                }
                
                list.RemoveAt(index);
            }
            
            using(Stream stream = File.Open(fileName, FileMode.Truncate))
                stream.Write(pb);
                
            Console.WriteLine("Package {0} updated.", fileName);
        }
    }
}