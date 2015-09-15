using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supido.Templates.Parsing
{
    public class Node
    {
        #region - Properties -

        public string Name { get; set; }

        public string Input { get; set; }

        public int Begin { get; set; }

        public int End { get; set; }

        public IList<Node> Nodes { get; set; }

        #endregion

        public Node(string name, ParserState state)
        {
            if (name == null)
            {
                throw new ArgumentNullException("Name of node cannot be null");
            }
            this.Name = name;
            this.Input = state.Input;
            this.Begin = state.CurrentPosition;
            this.Nodes = new List<Node>();
        }
    }
}
