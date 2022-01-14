using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class HttpWebRequestTest
    {
        //共通設定
        public int iTimeout = 3000;
        public Encoding MyEncoding = Encoding.UTF8;
        public string Path = System.IO.Directory.GetCurrentDirectory() + "/";
        

        public Stream getResponseStream(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            request.Timeout = iTimeout;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream streamReceive = response.GetResponseStream();
            return streamReceive;
        }

        /// <summary>
        /// 正則
        /// ex
        /// https?\:\/\/[^\"\<\>\(\)\）\（\']*
        /// https?\:\/\/[^\"\<\>\）\（\']*
        /// </summary>
        public List<string> getRegexList(string txt, string pattern)
        {
            Regex Regex = new Regex(pattern);

            MatchCollection MM = Regex.Matches(txt);

            List<string> Re = new List<string>();
            foreach (Match it in MM)
            {
                Re.Add(it.Value);
            }

            return Re;
        }

        public string getString(Stream Response, Encoding enc = null)
        {
            StreamReader streamReader = null;
            Encoding thisEnc = enc;
            try
            {
                if (Response == null) return "";
                if (thisEnc == null) thisEnc = MyEncoding;
                streamReader = new StreamReader(Response, thisEnc);
                string strResult = streamReader.ReadToEnd();
                return strResult;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (streamReader != null) streamReader.Dispose();
                Response.Dispose();
            }
        }




        /// <summary>
        /// 存成檔案
        /// </summary>
        public void getFile(Stream Response, string FileName)
        {
            FileStream Fs = null;
            try
            {
                if (Response == null) return;
                Fs = File.Create(Path + FileName);
                Response.CopyTo(Fs);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (Fs != null) Fs.Dispose();
                Response.Dispose();
            }


        }

        public void getRegexFile(List<string> List, string FileName, Encoding enc = null)
        {
            StreamWriter sw = null;
            Encoding thisEnc = enc;
            try
            {
                if (thisEnc == null) thisEnc = MyEncoding;
                sw = new StreamWriter(Path + FileName, false, thisEnc);
                foreach (var it in List)
                {
                    sw.WriteLine(it);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
        }

        public void getStringFile(string input, string FileName, Encoding enc = null)
        {
            StreamWriter sw = null;
            Encoding thisEnc = enc;
            try
            {
                if (thisEnc == null) thisEnc = MyEncoding;
                sw = new StreamWriter(Path + FileName, false, thisEnc);
                sw.Write(input);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
        }


        /// <summary>
        /// 資料夾
        /// </summary>
        /// <param name="sName"></param>
        public void CreateDirectory(string sName)
        {
            if (!Directory.Exists(Path + sName))
            {
                Directory.CreateDirectory(Path + "\\" + sName + "\\");
            }
        }
        public List<string> getFileList(string sPath)
        {
            return Directory.GetFileSystemEntries(sPath).ToList();
        }
        
        

    }
}
