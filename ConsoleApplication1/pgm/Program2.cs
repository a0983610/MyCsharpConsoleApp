using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program2
    {
        static void Main(string[] args)
        {
            StreamReader sr = null;
            StreamWriter sw = null;
            try
            {
                string PGPath = System.IO.Directory.GetCurrentDirectory();

                if (File.Exists(PGPath + "\\Target.txt"))
                {
                    sr = new StreamReader(PGPath + "\\Target.txt");
                }
                else
                {
                    //說明
                    sw = new StreamWriter(PGPath + "\\Target.txt");
                    sw.WriteLine("[(第一行)圖片檔資料夾路徑]");
                    sw.WriteLine("=[設定要加上的文字]");
                    sw.WriteLine("=size:[文字大小(數字)預設22]");
                    sw.WriteLine("=w:[文字區寬度(數字)預設200]");
                    sw.WriteLine("=h:[文字區高度(數字)預設50]");
                    sw.WriteLine("=DN:[設定儲存檔案資料夾名稱]");
                    sw.WriteLine("[檔名]");
                    sw.Flush();
                    sw.Dispose();
                    return;
                }
                
                string sLine = string.Empty;

                string FLPath = sr.ReadLine() + "\\";
                string setStr = "";
                sLine = sr.ReadLine();

                ImageTest ImageTest = new ImageTest();
                
                while (!string.IsNullOrEmpty(sLine))
                {
                    if (sLine.StartsWith("="))
                    {
                        if (sLine.StartsWith("=size:"))
                        {
                            //設定文字大小
                            string size = sLine.Substring(6);
                            ImageTest.FontEmsize = Convert.ToInt32(size);
                        }
                        else if (sLine.StartsWith("=w:"))
                        {
                            string w = sLine.Substring(3);
                            ImageTest.width = Convert.ToSingle(w);
                        }
                        else if (sLine.StartsWith("=h:"))
                        {
                            string h = sLine.Substring(3);
                            ImageTest.height = Convert.ToSingle(h);
                        }
                        else if (sLine.StartsWith("=DN:"))
                        {
                            string DN = sLine.Substring(4);
                            ImageTest.DirectoryName = DN;
                        }
                        else
                        {
                            //設定文字
                            setStr = sLine.Substring(1);
                        }
                    }
                    else
                    {
                        sLine = fixfileName(FLPath, sLine);
                        if(File.Exists(FLPath + sLine))
                        {
                            ImageTest.ImageDrawString(setStr, FLPath + sLine);
                        }
                    }

                    sLine = sr.ReadLine();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("發生錯誤:\n" + ex);
                Console.ReadKey();
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
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
