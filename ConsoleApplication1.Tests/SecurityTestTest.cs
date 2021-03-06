// <copyright file="SecurityTestTest.cs">Copyright ©  2018</copyright>

using System;
using ConsoleApplication1;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleApplication1.Tests
{
    [TestClass]
    [PexClass(typeof(SecurityTest))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class SecurityTestTest
    {

        [PexMethod]
        public byte[] Md5(byte[] bData)
        {
            byte[] result = SecurityTest.Md5(bData);
            return result;
            // TODO: 將判斷提示加入 方法 SecurityTestTest.Md5(Byte[])
        }
    }
}
