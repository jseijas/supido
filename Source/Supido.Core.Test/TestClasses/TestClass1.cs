using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supido.Core.Test.TestClasses
{
    public class TestClass1
    {
        #region - Fields -

        public int field;

        #endregion

        #region - Properties -

        public long Id { get; set; }

        public string Name { get; set; }

        #endregion

        #region - Constructors -

        public TestClass1()
        {
        }

        public TestClass1(long id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        #endregion

        #region - Methods -

        public void DoNothing()
        {
        }

        #endregion

    }
}
