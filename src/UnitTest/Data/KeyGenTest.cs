
using System;
using System.Collections.Generic;
using System.Text;
using Lephone.Data;
using Lephone.Data.Caching;
using NUnit.Framework;

namespace Lephone.UnitTest.Data
{
    [TestFixture]
    public class KeyGenTest
    {
        [Test]
        public void Test1()
        {
            KeyGenerator k = new KeyGenerator();
            string s = k.GetKey(typeof(KeyGenTest), 25);
            Assert.AreEqual("Lephone.UnitTest.Data.KeyGenTest,25", s);
        }

        [Test]
        public void Test2()
        {
            KeyGenerator k = new FullKeyGenerator();
            string s = k.GetKey(typeof(KeyGenTest), 25);
            string a = typeof(KeyGenTest).Assembly.FullName;
            Assert.AreEqual(a + ",Lephone.UnitTest.Data.KeyGenTest,25", s);
        }
    }
}