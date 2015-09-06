using Microsoft.VisualStudio.TestTools.UnitTesting;
using Supido.Core.Proxy;
using Supido.Core.Test.TestClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supido.Core.Test.Proxy
{
    [TestClass]
    public class ObjectProxyTest
    {
        [TestMethod]
        public void TestObjectProxyConstructor()
        {
            ObjectProxy proxy = new ObjectProxy(typeof(TestClass1));
            Assert.IsNotNull(proxy);
            Assert.AreEqual(typeof(TestClass1), proxy.ProxyType);
            Assert.AreEqual(typeof(List<TestClass1>), proxy.CollectionType);
        }

        [TestMethod]
        public void TestObjectProxyPropertyNames()
        {
            ObjectProxy proxy = new ObjectProxy(typeof(TestClass1));
            Assert.IsNotNull(proxy);
            string[] propertyNames = proxy.PropertyNames;
            Assert.AreEqual(2, propertyNames.Length);
            Assert.AreEqual("Id", propertyNames[0]);
            Assert.AreEqual("Name", propertyNames[1]);
        }

        [TestMethod]
        public void TestObjectProxyGetValue()
        {
            ObjectProxy proxy = new ObjectProxy(typeof(TestClass1));
            TestClass1 instance = new TestClass1(7L, "test");
            Assert.AreEqual(7L, proxy.GetValue(instance, "id"));
            Assert.AreEqual(7L, proxy.GetValue(instance, "Id"));
            Assert.AreEqual(7L, proxy.GetValue(instance, "ID"));
            Assert.AreEqual("test", proxy.GetValue(instance, "name"));
            Assert.AreEqual("test", proxy.GetValue(instance, "Name"));
            Assert.AreEqual("test", proxy.GetValue(instance, "NAME"));
        }

        [TestMethod]
        public void TestObjectProxySetValue()
        {
            ObjectProxy proxy = new ObjectProxy(typeof(TestClass1));
            TestClass1 instance = new TestClass1();
            proxy.SetValue(instance, "id", 7L);
            Assert.AreEqual(7L, instance.Id);
            proxy.SetValue(instance, "Id", 7L);
            Assert.AreEqual(7L, instance.Id);
            proxy.SetValue(instance, "id", 9);
            Assert.AreEqual(9L, instance.Id);
            proxy.SetValue(instance, "id", "11");
            Assert.AreEqual(11L, instance.Id);
            proxy.SetValue(instance, "name", "test");
            Assert.AreEqual("test", instance.Name);
            proxy.SetValue(instance, "Name", "test");
            Assert.AreEqual("test", instance.Name);
        }

        [TestMethod]
        public void TestObjectProxyCreateObject()
        {
            ObjectProxy proxy = new ObjectProxy(typeof(TestClass1));
            TestClass1 instance = (TestClass1)proxy.CreateObject();
            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public void TestObjectProxyCreateList()
        {
            ObjectProxy proxy = new ObjectProxy(typeof(TestClass1));
            IList<TestClass1> list = (IList<TestClass1>)proxy.CreateList();
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void TestObjectProxyGetPropertyType()
        {
            ObjectProxy proxy = new ObjectProxy(typeof(TestClass1));
            Assert.AreEqual(typeof(long), proxy.GetPropertyType("id"));
            Assert.AreEqual(typeof(string), proxy.GetPropertyType("name"));
        }

        [TestMethod]
        public void TestObjectProxyIsNullProperties()
        {
            ObjectProxy proxy = new ObjectProxy(typeof(TestClass2));
            TestClass2 instance2 = new TestClass2();
            Assert.IsTrue(proxy.IsNullProperties(instance2));
            instance2.Age = 7;
            Assert.IsFalse(proxy.IsNullProperties(instance2));
            proxy = new ObjectProxy(typeof(TestClass1));
            TestClass1 instance1 = new TestClass1();
            Assert.IsFalse(proxy.IsNullProperties(instance1));
        }

        [TestMethod]
        public void TestObjectProxyIsDefaultProperties()
        {
            ObjectProxy proxy = new ObjectProxy(typeof(TestClass2));
            TestClass2 instance2 = new TestClass2();
            Assert.IsTrue(proxy.IsDefaultProperties(instance2));
            instance2.Age = 7;
            Assert.IsFalse(proxy.IsDefaultProperties(instance2));
            proxy = new ObjectProxy(typeof(TestClass1));
            TestClass1 instance1 = new TestClass1();
            Assert.IsTrue(proxy.IsDefaultProperties(instance1));
            instance1.Id = 7;
            Assert.IsFalse(proxy.IsDefaultProperties(instance1));
        }

        [TestMethod]
        public void TestObjectProxyWriteToMap()
        {
            TestClass1 instance = new TestClass1(7L, "test");
            ObjectProxy proxy = new ObjectProxy(typeof(TestClass1));
            Dictionary<string, object> map = proxy.WriteToMap(instance);
            Assert.IsNotNull(map);
            Assert.AreEqual(2, map.Count);
            Assert.AreEqual(7L, map["Id"]);
            Assert.AreEqual("test", map["Name"]);
        }

        [TestMethod]
        public void TestObjectProxyWriteToStringMap()
        {
            TestClass1 instance = new TestClass1(7L, "test");
            ObjectProxy proxy = new ObjectProxy(typeof(TestClass1));
            Dictionary<string, string> map = proxy.WriteToStringMap(instance);
            Assert.IsNotNull(map);
            Assert.AreEqual(2, map.Count);
            Assert.AreEqual("7", map["Id"]);
            Assert.AreEqual("test", map["Name"]);
        }

        [TestMethod]
        public void TestObjectProxyReadFromMap()
        {
            // Normal Test
            Dictionary<string, object> map = new Dictionary<string, object>();
            map.Add("Id", 7L);
            map.Add("Name", "test");
            ObjectProxy proxy = new ObjectProxy(typeof(TestClass1));
            TestClass1 instance = new TestClass1();
            proxy.ReadFromMap(instance, map);
            Assert.AreEqual(7L, instance.Id);
            Assert.AreEqual("test", instance.Name);
            // Lower case test
            map = new Dictionary<string, object>();
            map.Add("id", 7L);
            map.Add("name", "test");
            instance = new TestClass1();
            proxy.ReadFromMap(instance, map);
            Assert.AreEqual(7L, instance.Id);
            Assert.AreEqual("test", instance.Name);
            // Convert types test
            map = new Dictionary<string, object>();
            map.Add("id", "7");
            map.Add("name", "test");
            instance = new TestClass1();
            proxy.ReadFromMap(instance, map);
            Assert.AreEqual(7L, instance.Id);
            Assert.AreEqual("test", instance.Name);
        }

        [TestMethod]
        public void TestObjectProxyReadFromStringMap()
        {
            // Normal Test
            Dictionary<string, string> map = new Dictionary<string, string>();
            map.Add("Id", "7");
            map.Add("Name", "test");
            ObjectProxy proxy = new ObjectProxy(typeof(TestClass1));
            TestClass1 instance = new TestClass1();
            proxy.ReadFromStringMap(instance, map);
            Assert.AreEqual(7L, instance.Id);
            Assert.AreEqual("test", instance.Name);
            // Lower Case Test
            map = new Dictionary<string, string>();
            map.Add("id", "7");
            map.Add("name", "test");
            instance = new TestClass1();
            proxy.ReadFromStringMap(instance, map);
            Assert.AreEqual(7L, instance.Id);
            Assert.AreEqual("test", instance.Name);
        }

        [TestMethod]
        public void TestObjectProxyCloneObject()
        {
            ObjectProxy proxy = new ObjectProxy(typeof(TestClass1));
            TestClass1 instance = new TestClass1(7L, "test");
            TestClass1 other = (TestClass1)proxy.CloneObject(instance);
            Assert.IsNotNull(other);
            Assert.AreEqual(7L, other.Id);
            Assert.AreEqual("test", other.Name);
        }

    }
}
