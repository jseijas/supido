using Supido.Templates.Parsing.Grammars;
using System;

namespace Supido.Templates.Parsing.Evaluators
{
    /// <summary>
    /// Evaluate an arithmetic expression.
    /// </summary>
    public class ArithmeticEvaluator
    {
        #region - Methods -

        /// <summary>
        /// Evaluates a given string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static dynamic Eval(string s)
        {
            return Eval(ArithmeticGrammar.Expression.Parse(s)[0]);
        }

        /// <summary>
        /// Evaluates a given node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Unreocognized operator  + node[1].Text
        /// or
        /// or
        /// Unexpected type of node  + node.Name
        /// </exception>
        public static dynamic Eval(Token node)
        {
            switch (node.Name)
            {
                case "Number": return Eval(node[0]);
                case "Integer": return Int64.Parse(node.Text);
                case "Float": return Double.Parse(node.Text);
                case "PrefixExpr":
                    switch (node[0].Text)
                    {
                        case "-": return -Eval(node[1]);
                        case "!": return !Eval(node[1]);
                        case "~": return ~Eval(node[1]);
                        default: throw new Exception(node[0].Text);
                    }
                case "ParanExpr": return Eval(node[0]);
                case "Expression":
                    switch (node.NodeCount)
                    {
                        case 1:
                            return Eval(node[0]);
                        case 3:
                            switch (node[1].Text)
                            {
                                case "+": return Eval(node[0]) + Eval(node[2]);
                                case "-": return Eval(node[0]) - Eval(node[2]);
                                case "*": return Eval(node[0]) * Eval(node[2]);
                                case "/": return Eval(node[0]) / Eval(node[2]);
                                case "%": return Eval(node[0]) % Eval(node[2]);
                                case "<<": return Eval(node[0]) << Eval(node[2]);
                                case ">>": return Eval(node[0]) >> Eval(node[2]);
                                case "==": return Eval(node[0]) == Eval(node[2]);
                                case "!=": return Eval(node[0]) != Eval(node[2]);
                                case "<=": return Eval(node[0]) <= Eval(node[2]);
                                case ">=": return Eval(node[0]) >= Eval(node[2]);
                                case "<": return Eval(node[0]) < Eval(node[2]);
                                case ">": return Eval(node[0]) > Eval(node[2]);
                                case "&&": return Eval(node[0]) && Eval(node[2]);
                                case "||": return Eval(node[0]) || Eval(node[2]);
                                case "&": return Eval(node[0]) & Eval(node[2]);
                                case "|": return Eval(node[0]) | Eval(node[2]);
                                default: throw new Exception("Unreocognized operator " + node[1].Text);
                            }
                        default:
                            throw new Exception(string.Format("Unexpected number of nodes {0} in expression", node.NodeCount));
                    }
                default: throw new Exception("Unexpected type of node " + node.Name);
            }
        }

        #endregion
    }
}
