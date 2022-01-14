using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections;
using System.IO.Compression;
using System.Collections.Specialized;

namespace ConsoleApplication1
{
    static class UriTest
    {
        static void Main(string[] args)
        {
            try
            {
                string uri = Console.ReadLine();
                tryUri(uri, 0, 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }
            finally
            {

            }
        }


        static string getSrcUri(string uri,string src)
        {
            try
            {
                if (src.StartsWith("//"))
                {
                    if (uri.StartsWith("https:"))
                    {
                        return "https:" + src;
                    }
                    else
                    {
                        return "http:" + src;
                    }
                }
                else if (src.StartsWith("./"))
                {
                    string tmp = "";
                    src = src.Substring(2);
                    int L = uri.LastIndexOf("/");
                    tmp = uri.Substring(0, L);
                    return tmp + "/" + src;
                }
                else if (src.StartsWith("../"))
                {
                    string tmp = "";
                    src = src.Substring(3);
                    int L = uri.LastIndexOf("/");
                    tmp = uri.Substring(0, L);
                    L = tmp.LastIndexOf("/");
                    tmp = uri.Substring(0, L);
                    return tmp + "/" + src;
                }
                else if (src.StartsWith("/"))
                {
                    string tmp = "";
                    int L = uri.IndexOf("/", 8);
                    if (L != -1)
                    {
                        tmp = uri.Substring(0, L);
                    }
                    else
                    {
                        tmp = uri;
                    }

                    return tmp + src;
                }
                else
                {
                    string tmp = "";
                    int L = uri.LastIndexOf("/");
                    tmp = uri.Substring(0, L);

                    return tmp + "/" + src;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("相對路徑錯誤");
                return "";
            }
        }


        static void tryUri(string uri, int iLevel = 0, int iMaxLevel = 0)
        {
            try
            {
                if (iLevel > iMaxLevel) return;
                
                HttpWebRequestTest req = new HttpWebRequestTest();
                req.iTimeout = 1000;

                string sResponse = req.getString(req.getResponseStream(uri));
                List<string> uriList = req.getRegexList(sResponse, "https?\\:\\/\\/[^\\\"\\<\\>\\）\\（\\']*");

                List<string> imgsrcList = req.getRegexList(sResponse, "img[^>]*src=['\"][^'\"]*");
                List<string> ahrefList = req.getRegexList(sResponse, "a[^>]*href=['\"][^'\"]*");

                if (uriList.Count == 0)
                {
                    sResponse = req.getString(req.getResponseStream(uri), Encoding.GetEncoding(950));
                    uriList = req.getRegexList(sResponse, "https?\\:\\/\\/[^\\\"\\<\\>\\）\\（\\']*");
                }

                if (uriList.Count == 0) return;

                /*測試
                req.getRegexFile(ahrefList, "a.txt");
                req.getRegexFile(imgsrcList, "imgsrcList.txt");
                req.getRegexFile(uriList, "uriList.txt");
                req.getStringFile(sResponse, "Response.txt");
                return;
                //*/

                foreach (string it in imgsrcList)
                {
                    int L = it.IndexOf("src=");
                    string tmp = it.Substring(L + 5);
                    if (tmp.StartsWith("http")) continue;

                    uriList.Add(getSrcUri(uri, tmp));
                }

                foreach(string it in ahrefList)
                {
                    int L = it.IndexOf("href=");
                    string tmp = it.Substring(L + 6);
                    if (tmp.StartsWith("http")) continue;

                    uriList.Add(getSrcUri(uri, tmp));
                }


                //檔案&資料夾
                string sFilePath = req.Path + "\\File\\";
                req.CreateDirectory("File");
                
                var FileList = req.getFileList(sFilePath);

                int i = 0;
                if (FileList.Count > 0) i = FileList.Count;
                //******

                //目標
                List<string> sFile = new List<string>();
                sFile.Add(".jpg");
                sFile.Add(".png");
                sFile.Add(".gif");
                //sFile.Add(".pdf");
                //sFile.Add(".doc");
                //sFile.Add(".xls");
                //sFile.Add(".docx");
                //sFile.Add(".xlsx");

                uriList = uriList.AsEnumerable().Distinct().ToList();
                bool isInsFile = false;

                foreach (var it in uriList)
                {
                    isInsFile = false;
                    foreach (var itf in sFile)
                    {
                        if (it.IndexOf(itf) != -1 || it.IndexOf(itf.ToUpper()) != -1)
                        {
                            System.Threading.Thread.Sleep(1000);
                            Console.WriteLine(it);
                            isInsFile = true;
                            try
                            {
                                req.getFile(req.getResponseStream(it), "\\File\\" + i + itf);
                                i++;
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("找不到");
                            }
                        }
                    }
                    if (!isInsFile)
                    {
                        if (!(iLevel + 1 > iMaxLevel)) tryUri(it, iLevel + 1, iMaxLevel);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        
    }
}
