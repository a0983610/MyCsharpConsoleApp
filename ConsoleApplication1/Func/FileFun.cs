using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class FileFun
    {
        public static void WriteObj(object obj, string Path)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (StreamWriter sw = new StreamWriter(Path))
            {
                bf.Serialize(sw.BaseStream, obj);
            }
        }

        public static object ReadObj(object obj, string Path)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (StreamReader sr = new StreamReader(Path))
            {
                return bf.Deserialize(sr.BaseStream);
            }
        }

        public static void CreateDirectory(string Path)
        {
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
        }
        
        public static string getBaseDirectory()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
