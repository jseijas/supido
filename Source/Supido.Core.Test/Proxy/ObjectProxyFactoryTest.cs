using AutoMapper;
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
    public class ObjectProxyFactoryTest
    {
        [ClassInitialize()]
        public static void ClassInit(TestContext context) 
        {
            ObjectProxyFactory.ConfigureMappers();
            IList<string> sourceNames = new List<string>();
            IList<string> targetNames = new List<string>();
            sourceNames.Add("Id");
            sourceNames.Add("Name");
            targetNames.Add("ObjectId");
            targetNames.Add("NameTxt");
            ObjectProxyFactory.CreateMap(typeof(TestClass1), typeof(TestClass3), sourceNames, targetNames);
        }

        [TestMethod]
        public void TestObjectProxyFactoryGetProxy()
        {
            IObjectProxy proxy1 = ObjectProxyFactory.Get<TestClass1>();
            Assert.IsNotNull(proxy1);
            Assert.AreEqual(typeof(TestClass1), proxy1.ProxyType);
            IObjectProxy proxy2 = ObjectProxyFactory.GetByType(typeof(TestClass1));
            Assert.AreSame(proxy1, proxy2);
            TestClass1 instance = new TestClass1();
            IObjectProxy proxy3 = ObjectProxyFactory.Get(instance);
            Assert.AreSame(proxy1, proxy3);
        }

        [TestMethod]
        public void TestObjectProxyFactoryMap()
        {
            TestClass1 source = new TestClass1(7L, "hola");
            TestClass3 target = ObjectProxyFactory.MapTo<TestClass3>(source);
            Assert.AreEqual(source.Id, target.ObjectId);
            Assert.AreEqual(source.Name, target.NameTxt);
            TestClass1 newSource = ObjectProxyFactory.MapTo<TestClass1>(target);
            Assert.AreEqual(source.Id, newSource.Id);
            Assert.AreEqual(source.Name, newSource.Name);
        }



        //[TestMethod]
        //public void TestObjectProxyFactoryMapToList()
        //{
        //    ObjectProxyFactory.CreateMap(typeof(TestClass1), typeof(TestClass3));

        //    //TestClass1 List to TestClass3 List
        //    IList<TestClass1> list1 = new List<TestClass1>();
        //    list1.Add(new TestClass1(1L, "name1"));
        //    list1.Add(new TestClass1(2L, "name2"));
        //    list1.Add(new TestClass1(3L, "name3"));
        //    IList<TestClass3> list3 = ObjectProxyFactory.MapToList<TestClass3>(list1);
        //    Assert.IsNotNull(list3);
        //    Assert.AreEqual(3, list3.Count);
        //    for (int i = 1; i <= 3; i++)
        //    {
        //        Assert.AreEqual(i, list3[i - 1].ObjectId);
        //        Assert.AreEqual("name" + i.ToString(), list3[i - 1].NameTxt);
        //    }

        //    //Converts back from TestClass3 list to TestClass1 list

        //    list1 = ObjectProxyFactory.MapToList<TestClass1>(list3);
        //    Assert.IsNotNull(list1);
        //    Assert.AreEqual(3, list1.Count);
        //    for (int i = 1; i <= 3; i++)
        //    {
        //        Assert.AreEqual(i, list1[i - 1].Id);
        //        Assert.AreEqual("name" + i.ToString(), list1[i - 1].Name);
        //    }

        //    //Converts from TestClass1 to TestClass2 without mapper
        //    IList<TestClass2> list2 = ObjectProxyFactory.MapToList<TestClass2>(list1);
        //    Assert.IsNotNull(list2);
        //    Assert.AreEqual(3, list3.Count);
        //    for (int i = 1; i <= 3; i++)
        //    {
        //        Assert.AreEqual(i, list2[i - 1].Id);
        //        Assert.AreEqual("name" + i.ToString(), list2[i - 1].Name);
        //        Assert.IsNull(list2[i - 1].Age);
        //    }

        //    // Converts back from TestClass2 to TestClass1 without mapper
        //    list1 = ObjectProxyFactory.MapToList<TestClass1>(list2);
        //    Assert.IsNotNull(list1);
        //    Assert.AreEqual(3, list1.Count);
        //    for (int i = 1; i <= 3; i++)
        //    {
        //        Assert.AreEqual(i, list1[i - 1].Id);
        //        Assert.AreEqual("name" + i.ToString(), list1[i - 1].Name);
        //    }

        //    // Converts a mixture objects to a TestClass1 list
        //    IList mixture = new List<object>();
        //    mixture.Add(new TestClass1(1L, "name1"));
        //    mixture.Add(new TestClass2(2, "name2", 9));
        //    mixture.Add(new TestClass3(3, "name3"));
        //    list1 = ObjectProxyFactory.MapToList<TestClass1>(mixture);
        //    Assert.IsNotNull(list1);
        //    Assert.AreEqual(3, list1.Count);
        //    for (int i = 1; i <= 3; i++)
        //    {
        //        Assert.AreEqual(i, list1[i - 1].Id);
        //        Assert.AreEqual("name" + i.ToString(), list1[i - 1].Name);
        //    }

        //    // Converts a mixture objects to a TestClass3 list
        //    list3 = ObjectProxyFactory.MapToList<TestClass3>(mixture);
        //    Assert.IsNotNull(list3);
        //    Assert.AreEqual(3, list3.Count);
        //    for (int i = 1; i <= 3; i++)
        //    {
        //        if (i == 2)
        //        {
        //            Assert.AreEqual(0, list3[i - 1].ObjectId);
        //            Assert.IsNull(list3[i - 1].NameTxt);
        //        }
        //        else
        //        {
        //            Assert.AreEqual(i, list3[i - 1].ObjectId);
        //            Assert.AreEqual("name" + i.ToString(), list3[i - 1].NameTxt);
        //        }
        //    }
        //}

        [TestMethod]
        public void TestObjectProxyFactoryToMap()
        {
            TestClass1 instance = new TestClass1(7L, "test");
            Dictionary<string, object> map = ObjectProxyFactory.ToMap(instance);
            Assert.IsNotNull(map);
            Assert.AreEqual(2, map.Count);
            Assert.AreEqual(7L, map["Id"]);
            Assert.AreEqual("test", map["Name"]);
        }

        [TestMethod]
        public void TestObjectProxyFactoryToStringMap()
        {
            TestClass1 instance = new TestClass1(7L, "test");
            Dictionary<string, string> map = ObjectProxyFactory.ToStringMap(instance);
            Assert.IsNotNull(map);
            Assert.AreEqual(2, map.Count);
            Assert.AreEqual("7", map["Id"]);
            Assert.AreEqual("test", map["Name"]);
        }

        [TestMethod]
        public void TestObjectProxyFactoryFromMap()
        {
            // Normal Test
            Dictionary<string, object> map = new Dictionary<string, object>();
            map.Add("Id", 7L);
            map.Add("Name", "test");
            TestClass1 instance = ObjectProxyFactory.FromMap<TestClass1>(map);
            Assert.AreEqual(7L, instance.Id);
            Assert.AreEqual("test", instance.Name);
            // Lower case test
            map = new Dictionary<string, object>();
            map.Add("id", 7L);
            map.Add("name", "test");
            instance = ObjectProxyFactory.FromMap<TestClass1>(map);
            Assert.AreEqual(7L, instance.Id);
            Assert.AreEqual("test", instance.Name);
            // Convert types test
            map = new Dictionary<string, object>();
            map.Add("id", "7");
            map.Add("name", "test");
            instance = ObjectProxyFactory.FromMap<TestClass1>(map);
            Assert.AreEqual(7L, instance.Id);
            Assert.AreEqual("test", instance.Name);
        }

        [TestMethod]
        public void TestObjectProxyFactoryFromStringMap()
        {
            // Normal Test
            Dictionary<string, string> map = new Dictionary<string, string>();
            map.Add("Id", "7");
            map.Add("Name", "test");
            TestClass1 instance = ObjectProxyFactory.FromStringMap<TestClass1>(map);
            Assert.AreEqual(7L, instance.Id);
            Assert.AreEqual("test", instance.Name);
            // Lower Case Test
            map = new Dictionary<string, string>();
            map.Add("id", "7");
            map.Add("name", "test");
            instance = ObjectProxyFactory.FromStringMap<TestClass1>(map);
            Assert.AreEqual(7L, instance.Id);
            Assert.AreEqual("test", instance.Name);
        }

        [TestMethod]
        public void TestObjectProxyFactoryToListMap()
        {
            IList<TestClass1> list1 = new List<TestClass1>();
            list1.Add(new TestClass1(1L, "name1"));
            list1.Add(new TestClass1(2L, "name2"));
            list1.Add(new TestClass1(3L, "name3"));

            IList<Dictionary<string, object>> table = ObjectProxyFactory.ToListMap(list1);
            Assert.IsNotNull(table);
            for (int i = 1; i <= 3; i++)
            {
                Dictionary<string, object> row = table[i - 1];
                Assert.IsNotNull(row);
                Assert.AreEqual(2, row.Count);
                Assert.AreEqual((long)i, row["Id"]);
                Assert.AreEqual("name" + i.ToString(), row["Name"]);
            }
        }

        [TestMethod]
        public void TestObjectProxyFactoryToListStringMap()
        {
            IList<TestClass1> list1 = new List<TestClass1>();
            list1.Add(new TestClass1(1L, "name1"));
            list1.Add(new TestClass1(2L, "name2"));
            list1.Add(new TestClass1(3L, "name3"));

            IList<Dictionary<string, string>> table = ObjectProxyFactory.ToListStringMap(list1);
            Assert.IsNotNull(table);
            for (int i = 1; i <= 3; i++)
            {
                Dictionary<string, string> row = table[i - 1];
                Assert.IsNotNull(row);
                Assert.AreEqual(2, row.Count);
                Assert.AreEqual(i.ToString(), row["Id"]);
                Assert.AreEqual("name" + i.ToString(), row["Name"]);
            }
        }

        [TestMethod]
        public void TestObjectProxyFactoryFromListMap()
        {
            IList<Dictionary<string, object>> table = new List<Dictionary<string, object>>();
            for (int i = 1; i <= 3; i++)
            {
                Dictionary<string, object> item = new Dictionary<string, object>();
                item.Add("Id", i);
                item.Add("Name", "name" + i.ToString());
                table.Add(item);
            }

            IList<TestClass1> list = ObjectProxyFactory.FromListMap<TestClass1>(table);
            Assert.IsNotNull(list);
            Assert.AreEqual(3, list.Count);
            for (int i = 1; i <= 3; i++)
            {
                TestClass1 item = list[i - 1];
                Assert.IsNotNull(item);
                Assert.AreEqual(i, item.Id);
                Assert.AreEqual("name" + i.ToString(), item.Name);
            }

        }

        [TestMethod]
        public void TestObjectProxyFactoryFromListStringMap()
        {
            IList<Dictionary<string, string>> table = new List<Dictionary<string, string>>();
            for (int i = 1; i <= 3; i++)
            {
                Dictionary<string, string> item = new Dictionary<string, string>();
                item.Add("Id", i.ToString());
                item.Add("Name", "name" + i.ToString());
                table.Add(item);
            }

            IList<TestClass1> list = ObjectProxyFactory.FromListStringMap<TestClass1>(table);
            Assert.IsNotNull(list);
            Assert.AreEqual(3, list.Count);
            for (int i = 1; i <= 3; i++)
            {
                TestClass1 item = list[i - 1];
                Assert.IsNotNull(item);
                Assert.AreEqual(i, item.Id);
                Assert.AreEqual("name" + i.ToString(), item.Name);
            }

        }

    }
}
