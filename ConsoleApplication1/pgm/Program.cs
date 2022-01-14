using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //取得目前程式位置
                string path = System.Environment.CurrentDirectory + "\\";

                List<string> checkfileName = new List<string>();
                checkfileName.Add("Doc.config");
                checkfileName.Add("doc.xml");
                checkfileName.Add("p.xml");
                checkfileName.Add("list.txt");
                if (!checkfile(path, checkfileName))
                {
                    Console.WriteLine("缺乏必要檔案");
                    Console.ReadKey();
                    return;
                }

                StreamReader srConfig = new StreamReader(path + "Doc.config");
                string XmlfileName = srConfig.ReadLine().Substring(12);
                string lstPath = srConfig.ReadLine().Substring(9);
                string w = srConfig.ReadLine().Substring(6);
                string h = srConfig.ReadLine().Substring(7);
                srConfig.Close();

                string doc = ReadToEnd(path + "doc.xml");
                string P = ReadToEnd(path + "p.xml");

                StreamReader lst = new StreamReader(path + "list.txt");
                string fileName = lst.ReadLine();

                StringBuilder sb = new StringBuilder();
                int c = 1;
                while (!string.IsNullOrEmpty(fileName))
                {
                    fileName = fixfileName(lstPath, fileName);
                    if (fileName == "")
                    {
                        fileName = lst.ReadLine();
                        continue;
                    }

                    Stream fs = new FileStream(lstPath + fileName, FileMode.Open);
                    int fsL = (int)fs.Length;
                    byte[] buf = new byte[fsL];
                    fs.Read(buf, 0, fsL);

                    string s64Data = Convert.ToBase64String(buf, Base64FormattingOptions.InsertLineBreaks);

                    string tmp = P;
                    tmp = tmp.Replace("{{DATA}}", s64Data);
                    tmp = tmp.Replace("{{ID}}", c.ToString());
                    tmp = tmp.Replace("{{width}}", w);
                    tmp = tmp.Replace("{{height}}", h);
                    sb.Append(tmp);

                    fs.Close();

                    fileName = lst.ReadLine();
                    c++;
                }
                lst.Close();

                StreamWriter sw = new StreamWriter(path + XmlfileName);
                doc = doc.Replace("{{BODY}}", sb.ToString());

                sw.Write(doc);

                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }

        /// <summary>
        /// 檢查必要檔案
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static bool checkfile(string path, List<string> fileName)
        {
            try
            {
                foreach (var it in fileName)
                {
                    if (!File.Exists(path + it)) return false;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }


        static string ReadToEnd(string path)
        {
            try
            {
                StreamReader sr = new StreamReader(path);
                string str = sr.ReadToEnd();
                sr.Close();
                return str;
            }
            catch (Exception)
            {
                throw;
            }

        }


        static string fixfileName(string path, string fileName)
        {
            try
            {
                string RE = fileName;
                if (File.Exists(path + RE))
                {
                    return RE;
                }
                else if (File.Exists(path + RE + ".jpg"))
                {
                    return RE + ".jpg";
                }
                else if (File.Exists(path + RE + ".png"))
                {
                    return RE + ".png";
                }
                return "";
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
