using Supido.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supido.Core.Test.TestClasses
{
    public class TestClass2
    {

        public int field;

        [StringValue("IdTest1")]
        [StringValue("IdTest2")]
        [StringValue("IdTest3")]
        public int? Id { get; set; }

        [StringValue("NameTest")]
        public string Name { get; set; }

        public int? Age { get; set; }

        public TestClass2()
        {
        }

        public TestClass2(int id, string name, int age)
        {
            this.Id = id;
            this.Name = name;
            this.Age = age;
        }
    }
}
