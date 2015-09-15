using System;
using System.Linq;

namespace Supido.Templates.Parsing.Evaluators
{
    /// <summary>
    /// Manages the transformations over the tokens.
    /// </summary>
    public class JavascriptTransformer : TreeTransformer
    {
        #region - Methods -

        /// <summary>
        /// Transforms the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public static Token Transform(Token token)
        {
            return new JavascriptTransformer().TransformAllTokens(token);
        }

        /// <summary>
        /// Transform to assignment, two operands and one operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="op">The op.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        private Token ToAssignment(Token left, string op, Token right)
        {
            return new Token("AssignExpr", left, new Token("AssignOp", "="), new Token("BinaryExpr", left, new Token("BinaryOp", op), right));
        }

        /// <summary>
        /// Returns the precedence order of an operator.
        /// </summary>
        /// <param name="op">The op.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Not a recognized operator</exception>
        private static int Precendence(string op)
        {
            switch (op)
            {
                case "=":
                case "+=":
                case "-=":
                case "*=":
                case "/=":
                case "%=":
                case ">>=":
                case "<<=":
                case "|=":
                case "&=":
                case "^=":
                case "||=":
                case "&&=":
                    return 10;
                case "||":
                    return 20;
                case "&&":
                    return 30;
                case "|":
                    return 40;
                case "^":
                    return 50;
                case "&":
                    return 60;
                case "==":
                case "!=":
                    return 70;
                case ">=":
                case "<=":
                case ">":
                case "<":
                    return 80;
                case ">>":
                case "<<":
                    return 90;
                case "+":
                case "-":
                    return 100;
                case "*":
                case "/":
                case "%":
                    return 110;
                default:
                    throw new Exception("Not a recognized operator");
            }
        }


        /// <summary>
        /// Splits a binary expression, using as pivot the lesser precedence operator.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Binary expression cannot have pair number of members</exception>
        private static Token SplitBinaryExpression(Token token)
        {
            if (token.NodeCount <= 3)
            {
                return token;
            }

            if (token.NodeCount % 2 == 0)
            {
                throw new Exception("Binary expression cannot have pair number of members");
            }
            int pivot = 1;
            for (int i = 3; i < token.NodeCount; i += 2)
            {
                if (Precendence(token[i].Text) < Precendence(token[pivot].Text)) 
                {
                    pivot = i;
                }
            }
            Token left = new Token("BinaryExpr", token.Tokens.Take(pivot).ToArray());
            Token right = new Token("BinaryExpr", token.Tokens.Skip(pivot + 1).ToArray());
            return new Token("BinaryExpr", SplitBinaryExpression(left), token[pivot], SplitBinaryExpression(right));
        }


        /// <summary>
        /// Internal transform of the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">The postfix epxression cannot have an empty token</exception>
        /// <exception cref="System.Exception">
        /// Unexptected token in postfix expression  + last.Name
        /// or
        /// Unexpected assignment operator  + token[1].Text
        /// </exception>
        protected override Token InternalTransform(Token token)
        {
            switch (token.Name)
            {
                case "Literal":
                case "LeafExpr":
                case "ParenExpr":
                case "Expr":
                case "Statement":
                case "ExprStatement":
                    return token[0];
                case "While":
                    return new Token("For", new Token("Empty"), token[0], new Token("Empty"), token[1]);
                case "PostfixExpr":
                    {
                        if (token.NodeCount == 0)
                        {
                            throw new ArgumentException("The postfix epxression cannot have an empty token");
                        }
                        if (token.NodeCount == 1)
                        {
                            return token.Tokens[0];
                        }
                        Token last = token.Tokens.Last();
                        switch (last.Name)
                        {
                            case "Field":
                                return this.LeftGroup(token, "FieldExpr");
                            case "Index":
                                return this.LeftGroup(token, "IndexExpr");
                            case "ArgList":
                                {
                                    Token call = this.LeftGroup(token, "CallExpr");
                                    if (call[0].Name == "FieldExpr")
                                    {
                                        return new Token("MethodCallExpr", call[0][0], call[0][1], call[1]);
                                    }
                                    else
                                    {
                                        return call;
                                    }
                                }
                            default:
                                throw new Exception("Unexptected token in postfix expression " + last.Name);
                        }
                    }
                case "NamedFunc":
                    return new Token("VarDecl", token[0], new Token("AnonFunc", token[1], token[2]));
                case "TertiaryExpr":
                    return (token.NodeCount == 1) ? token[0] : token;
                case "BinaryExpr":
                    return (token.NodeCount == 1) ? token[0] : SplitBinaryExpression(token);
                case "AssignExpr":
                    {
                        if (token.NodeCount == 1) 
                        {
                            return token[0];
                        }
                        switch (token[1].Text) 
                        {
                            case "=": return token;
                            case "+=": return ToAssignment(token[0], "+", token[2]);
                            case "-=": return ToAssignment(token[0], "-", token[2]);
                            case "*=": return ToAssignment(token[0], "*", token[2]);
                            case "/=": return ToAssignment(token[0], "/", token[2]);
                            case "%=": return ToAssignment(token[0], "%", token[2]);
                            case "|=": return ToAssignment(token[0], "|", token[2]);
                            case "&=": return ToAssignment(token[0], "&", token[2]);
                            case "^=": return ToAssignment(token[0], "^", token[2]);
                            case "||=": return ToAssignment(token[0], "||", token[2]);
                            case "&&=": return ToAssignment(token[0], "&&", token[2]);
                            case ">>=": return ToAssignment(token[0], ">>", token[2]);
                            case "<<=": return ToAssignment(token[0], "<<", token[2]);
                            default:
                                throw new Exception("Unexpected assignment operator " + token[1].Text);
                        }
                    }

            }
            return token;
        }

        #endregion
    }
}
