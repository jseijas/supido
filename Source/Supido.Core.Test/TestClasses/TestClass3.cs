using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supido.Core.Test.TestClasses
{
    public class TestClass3
    {
        public long ObjectId { get; set; }

        public string NameTxt { get; set; }

        public TestClass3()
        {
        }

        public TestClass3(long objectId, string nameTxt)
        {
            this.ObjectId = objectId;
            this.NameTxt = nameTxt;
        }
    }
}
