using Microsoft.VisualStudio.TestTools.UnitTesting;
using Supido.Core.Test.TestClasses;
using Supido.Core.Utils;
using System.Collections.Generic;

namespace Supido.Core.Test.Utils
{
    [TestClass]
    public class StringValueAttributeTest
    {
        [TestMethod]
        public void TestStringValueAttributeGet()
        {
            TestClass2 item = new TestClass2(7, "test", 0);
            string value = StringValueAttribute.Get(() => item.Id);
            Assert.IsNotNull(value);
            Assert.IsTrue(((string)value).StartsWith("IdTest"));
        }

        [TestMethod]
        public void TestStringValueAttributeGetAll()
        {
            TestClass2 item = new TestClass2(7, "test", 0);
            string[] values = StringValueAttribute.GetAll(() => item.Id);
            Assert.IsNotNull(values);
            List<string> list = new List<string>(values);
            Assert.IsTrue(list.IndexOf("IdTest1") >= 0);
            Assert.IsTrue(list.IndexOf("IdTest2") >= 0);
            Assert.IsTrue(list.IndexOf("IdTest3") >= 0);

        }
    }
}
