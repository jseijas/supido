using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Supido.Core.Test.TestClasses;
using System.Reflection;
using Supido.Core.Utils;
using System.Collections.Generic;

namespace Supido.Core.Test.Utils
{
    [TestClass]
    public class AttributeUtilTest
    {
        [TestMethod]
        public void TestAttributeUtilGetPropertyInfo()
        {
            TestClass2 item = new TestClass2(7, "test", 0);
            PropertyInfo pi = AttributeUtil.GetPropertyInfo(() => item.Id);
            Assert.IsNotNull(pi);
            Assert.AreEqual("Id", pi.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Expression 'field' refers to a field, not a property.")]
        public void TestAttributeUtilGetPropertyInfoNonProperty()
        {
            TestClass2 item = new TestClass2(7, "test", 0);
            PropertyInfo pi = AttributeUtil.GetPropertyInfo(() => item.field);
        }

        [TestMethod]
        public void TestAttributeUtilGetCustomAttribute()
        {
            TestClass2 item = new TestClass2(7, "test", 0);
            StringValueAttribute attribute = (StringValueAttribute)AttributeUtil.GetCustomAttribute(() => item.Id, typeof(StringValueAttribute));
            Assert.IsNotNull(attribute);
            Assert.IsTrue(attribute.Value.StartsWith("IdTest"));
            attribute = (StringValueAttribute)AttributeUtil.GetCustomAttribute(() => item.Name, typeof(StringValueAttribute));
            Assert.IsNotNull(attribute);
            Assert.AreEqual("NameTest", attribute.Value);
            attribute = (StringValueAttribute)AttributeUtil.GetCustomAttribute(() => item.Age, typeof(StringValueAttribute));
            Assert.IsNull(attribute);
        }

        [TestMethod]
        public void TestAttributeUtilGet()
        {
            TestClass2 item = new TestClass2(7, "test", 0);
            object value = AttributeUtil.Get(() => item.Id, typeof(StringValueAttribute), "Value");
            Assert.IsNotNull(value);
            Assert.IsTrue(((string)value).StartsWith("IdTest"));
            value = AttributeUtil.Get(() => item.Name, typeof(StringValueAttribute), "Value");
            Assert.AreEqual("NameTest", value);
            value = AttributeUtil.Get(() => item.Age, typeof(StringValueAttribute), "Value");
            Assert.IsNull(value);
        }

        [TestMethod]
        public void TestAttributeUtilGetCustomAttributes()
        {
            TestClass2 item = new TestClass2(7, "test", 0);
            StringValueAttribute[] attributes = (StringValueAttribute[])AttributeUtil.GetCustomAttributes(() => item.Id, typeof(StringValueAttribute));
            Assert.IsNotNull(attributes);
            Assert.AreEqual(3, attributes.Length);
            List<string> values = new List<string>();
            values.Add(attributes[0].Value);
            values.Add(attributes[1].Value);
            values.Add(attributes[2].Value);
            Assert.IsTrue(values.IndexOf("IdTest1") >= 0);
            Assert.IsTrue(values.IndexOf("IdTest2") >= 0);
            Assert.IsTrue(values.IndexOf("IdTest3") >= 0);
        }

        [TestMethod]
        public void TestAttributeUtilGetAll()
        {
            TestClass2 item = new TestClass2(7, "test", 0);
            object[] objvalues = AttributeUtil.GetAll(() => item.Id, typeof(StringValueAttribute), "Value");
            Assert.IsNotNull(objvalues);
            List<string> values = new List<string>();
            foreach (object obj in objvalues)
            {
                values.Add((string)obj);
            }
            Assert.IsTrue(values.IndexOf("IdTest1") >= 0);
            Assert.IsTrue(values.IndexOf("IdTest2") >= 0);
            Assert.IsTrue(values.IndexOf("IdTest3") >= 0);
        }

        [TestMethod]
        public void TestAttributeUtilGetAsString()
        {
            TestClass2 item = new TestClass2(7, "test", 0);
            string value = AttributeUtil.GetAsString(() => item.Id, typeof(StringValueAttribute), "Value");
            Assert.IsNotNull(value);
            Assert.IsTrue(((string)value).StartsWith("IdTest"));
            value = AttributeUtil.GetAsString(() => item.Name, typeof(StringValueAttribute), "Value");
            Assert.AreEqual("NameTest", value);
            value = AttributeUtil.GetAsString(() => item.Age, typeof(StringValueAttribute), "Value");
            Assert.IsNull(value);
        }

        [TestMethod]
        public void TestAttributeUtilGetAllAsString()
        {
            TestClass2 item = new TestClass2(7, "test", 0);
            string[] values = AttributeUtil.GetAllAsString(() => item.Id, typeof(StringValueAttribute), "Value");
            Assert.IsNotNull(values);
            Assert.AreEqual(3, values.Length);
            List<string> list = new List<string>(values);
            Assert.IsTrue(list.IndexOf("IdTest1") >= 0);
            Assert.IsTrue(list.IndexOf("IdTest2") >= 0);
            Assert.IsTrue(list.IndexOf("IdTest3") >= 0);
        }
    }
}
