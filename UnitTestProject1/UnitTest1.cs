using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApplication1;
using System.Text;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AES加解密指定Key()
        {
            string PW = "測試";
            
            string inputStr = "測試字串";
            byte[] Key = SecurityTest.UTF8StrToByte(PW, 32);
            byte[] IV = null;
            byte[] tmp = null;
            string outputStr;
            tmp = SecurityTest.AesE(inputStr, ref Key, ref IV);
            outputStr = SecurityTest.AesD(tmp, ref Key, ref IV);

            Assert.AreEqual(inputStr, outputStr);
        }

        [TestMethod]
        public void AES加解密隨機Key()
        {
            string inputStr = "測試字串";
            byte[] Key = null;
            byte[] IV = null;
            byte[] tmp = null;
            string outputStr;
            tmp = SecurityTest.AesE(inputStr, ref Key, ref IV);
            outputStr = SecurityTest.AesD(tmp, ref Key, ref IV);

            Assert.AreEqual(inputStr, outputStr);
        }

        [TestMethod]
        public void 雜湊測試()
        {
            string inputStr = "測試字串";
            byte[] input;
            byte[] outputSha256, outputSha1, outputMD5;
            input = Encoding.GetEncoding("big5").GetBytes(inputStr);
            outputSha256 = SecurityTest.Sha256(input);
            outputSha1 = SecurityTest.Sha1(input);
            outputMD5 = SecurityTest.Md5(input);

            Assert.IsNotNull(outputSha256, "Sha256:" + Encoding.GetEncoding("big5").GetString(outputSha256));
            Assert.IsNotNull(outputSha1, "Sha1:" + Encoding.GetEncoding("big5").GetString(outputSha1));
            Assert.IsNotNull(outputMD5, "MD5:" + Encoding.GetEncoding("big5").GetString(outputMD5));
        }

        [TestMethod]
        public void RSA加密測試()
        {
            string sKey = SecurityTest.RsaGetK();
            string sPubK = SecurityTest.RsaToPub(sKey);

            byte[] Edata = SecurityTest.UTF8StrToByte("測試資料", 32);
            Edata = SecurityTest.RsaE(Edata, sPubK, false);
            byte[] Ddata = SecurityTest.RsaD(Edata, sKey, false);
            string Str = SecurityTest.UTF8ByteToStr(Ddata);

            Assert.AreEqual("測試資料", Str);
        }
        
        [TestMethod]
        public void RSA簽證測試()
        {
            string sKey = SecurityTest.RsaGetK();
            string sPubK = SecurityTest.RsaToPub(sKey);
            byte[] Tdata = SecurityTest.UTF8StrToByte("測試資料", 20);
            byte[] Ttmp = SecurityTest.Sha256(Tdata);

            byte[] Fdata = SecurityTest.UTF8StrToByte("測試資料F", 20);
            byte[] Ftmp = SecurityTest.Sha256(Fdata);
            byte[] Sdata = SecurityTest.Signature(Ttmp, "SHA256", sKey);

            Assert.IsTrue(SecurityTest.SignatureC(Ttmp, Sdata, "SHA256", sPubK));
            Assert.IsFalse(SecurityTest.SignatureC(Ftmp, Sdata, "SHA256", sPubK));
        }
    }
}
