using Supido.Templates.Parsing.Grammars;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Supido.Templates.Parsing.Evaluators
{
    /// <summary>
    /// Evaluator for javascript language
    /// </summary>
    public class JavascriptEvaluator : Evaluator
    {
        #region - Fields -

        /// <summary>
        /// Indicates if the function is returning. 
        /// </summary>
        public bool isReturning = false;

        /// <summary>
        /// The result
        /// </summary>
        public dynamic result = null;

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="JavascriptEvaluator"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public JavascriptEvaluator(TemplateContainer container)
            : base(container)
        {
            this.AddVariable("alert", new JSPrimitive(args => { Console.WriteLine(args[0]); }));
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Evaluates the given string matching the given rule.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        public dynamic Eval(string s, Rule rule)
        {
            IList<Token> tokens = rule.Parse(s);
            return Eval(JavascriptTransformer.Transform(tokens[0]));
        }

        /// <summary>
        /// Evaluates a parser token list.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <returns></returns>
        public dynamic EvalTokens(IEnumerable<Token> tokens)
        {
            dynamic result = null;
            foreach (Token token in tokens)
            {
                result = Eval(token);
                if (isReturning)
                {
                    return result;
                }
            }
            return result;
        }


        /// <summary>
        /// Evals the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Evaluator cannot be in returning state when entering eval
        /// or
        /// Unrecognized prefix operator  + token[0].Text
        /// or
        /// Invalid left value  + token[0].Name
        /// or
        /// Unrecognized token type  + token.Name
        /// </exception>
        public dynamic Eval(Token token)
        {
            if (this.isReturning)
            {
                throw new Exception("Evaluator cannot be in returning state when entering eval");
            }
            switch (token.Name)
            {
                case "Return":
                    this.result = token.NodeCount == 1 ? Eval(token[0]) : null;
                    this.isReturning = true;
                    return result;
                case "Script":
                case "Block":
                    return this.EvalScoped(() => EvalTokens(token.Tokens));
                case "AnonFunc":
                    return new JSFunction(this.environment, token);
                case "If":
                    if (Eval(token[0]) ?? false)
                    {
                        return Eval(token[1]);
                    }
                    if (token.NodeCount > 2)
                    {
                        return Eval(token[2]);
                    }
                    return null;
                case "Else":
                    return Eval(token[0]);
                case "VarDecl":
                    return this.AddVariable(token[0].Text, token.NodeCount > 1 ? Eval(token[1]) : null);
                case "Empty":
                    return null;
                case "ExprtStatement":
                    return Eval(token[0]);
                case "For":
                    return EvalScoped(() =>
                    {
                        // Initialize...
                        Eval(token[0]);
                        dynamic r = null;
                        // Check condition...
                        while (!this.isReturning && (Eval(token[1]) ?? false))
                        {
                            // increment....
                            Eval(token[3]);
                            // execution...
                            r = Eval(token[2]);
                        }
                        return r;
                    });
                case "Foreach":
                    return EvalScoped(() =>
                        {
                            dynamic r = null;
                            object containerVar = this.container.GetByPath(token[1].Text);
                            if (containerVar is TemplateContainer)
                            {
                                TemplateContainer listContainer = (TemplateContainer)containerVar;
                                if (!listContainer.IsList)
                                {
                                    throw new Exception("container is not a list");
                                }
                                foreach (TemplateContainer value in listContainer.ArrayValues)
                                {
                                    this.container.AddChild(token[0].Text, value);
                                    r = Eval(token[2]);
                                }
                            }
                            else if (containerVar is TemplateLink)
                            {
                                TemplateLink listLInk = (TemplateLink)containerVar;
                                if (!listLInk.IsList)
                                {
                                    throw new Exception("container is not a list");
                                }
                                foreach (string path in listLInk.ListValues)
                                {
                                    string fullpath = listLInk.Link + "[" + path + "]";
                                    object foundobj = this.container.GetByPath(fullpath);
                                    if (foundobj is TemplateContainer)
                                    {
                                        this.container.AddChild(token[0].Text, foundobj as TemplateContainer);
                                        r = Eval(token[2]);
                                    }
                                }
                            }
                            return r;
                        });
                case "BinaryExpr":
                    {
                        return Primitives.Eval(token[1].Text, Eval(token[0]), Eval(token[2]));
                    }
                case "PrefixExpr":
                    switch (token[0].Text)
                    {
                        case "!":
                            return !Eval(token[1]);
                        case "~":
                            return ~Eval(token[1]);
                        case "-":
                            return -Eval(token[1]);
                        default:
                            throw new Exception("Unrecognized prefix operator " + token[0].Text);
                    }
                case "FieldExpr":
                    {
                        var obj = Eval(token[0]);
                        var field = token[1][0].Text;
                        if (obj is TemplateContainer)
                        {
                            return (obj as TemplateContainer).GetByPath(field);
                        }
                        else
                        {
                            return obj[field];
                        }
                    }
                case "IndexExpr":
                    {
                        var index = Eval(token[1]);
                        var array = Eval(token[0]);
                        if (array is TemplateContainer)
                        {
                            TemplateContainer tc = array as TemplateContainer;
                            TemplateContainer sontc = tc.GetByPath("[" + index.ToString() + "]");
                            return sontc;
                        }
                        else
                        {
                            return array[index];
                        }
                    }
                case "CallExpr":
                    {
                        var func = Eval(token[0]);
                        var args = token[1].Tokens.Select(Eval).ToArray();
                        return func.Apply(this, null, args);
                    }
                case "MethodCallExpr":
                    {
                        var obj = Eval(token[0]);
                        var func = Eval(token[1]);
                        var args = token[2].Tokens.Select(Eval).ToArray();
                        return func.Apply(this, obj, args);
                    }
                case "NewExpr":
                    {
                        var func = Eval(token[0]);
                        var args = token[1].Tokens.Select(Eval).ToArray();
                        return func.Apply(this, new JsonObject(), args);
                    }
                case "AssignExpr":
                    {
                        var lnode = token[0];
                        var rnode = token[2];
                        var rvalue = Eval(rnode);
                        switch (lnode.Name)
                        {
                            case "FieldExpr":
                                {
                                    var obj = Eval(lnode[0]);
                                    var name = lnode[1].Text;
                                    return obj[name] = rvalue;
                                }
                            case "IndexExpr":
                                {
                                    var obj = Eval(lnode[0]);
                                    var index = Eval(lnode[1]);
                                    return obj[index] = rvalue;
                                }
                            case "Identifier":
                                {
                                    var name = lnode.Text;
                                    return RebindGlobal(name, rvalue);
                                }
                            default:
                                throw new Exception("Invalid left value " + token[0].Name);
                        }
                    }
                case "Identifier":
                    if (this.environment.ExistsVariable(token.Text))
                    {
                        return this.environment[token.Text];
                    }
                    else
                    {
                        return this.container.GetByPath(token.Text);
                    }
                case "Object":
                    {
                        var r = new JsonObject();
                        foreach (var pair in token.Tokens)
                        {
                            var name = pair[0].Text;
                            var value = Eval(pair[1]);
                            r[name] = value;
                        }
                        return r;
                    }
                case "Array":
                    return token.Tokens.Select(Eval).ToArray();
                case "Integer":
                    return Int32.Parse(token.Text);
                case "Index":
                    return Eval(token[0]);
                case "Float":
                    return Double.Parse(token.Text);
                case "String":
                    return token.Text.Substring(1, token.Text.Length - 2);
                case "True":
                    return true;
                case "False":
                    return false;
                case "Null":
                    return null;
                default:
                    throw new Exception("Unrecognized token type " + token.Name);
            }
        }

        /// <summary>
        /// Runs the script.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        public static dynamic RunScript(string s, TemplateContainer container)
        {
            return new JavascriptEvaluator(container).Eval(s, JavascriptGrammar.Script);
        }

        /// <summary>
        /// Runs the script given in variable s, and returns a string result.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        public static string ReplaceString(string s, TemplateContainer container)
        {
            dynamic result = RunScript(s, container);
            if (result is string)
            {
                return (string)result;
            }
            else
            {
                return result.ToString();
            }
        }

        #endregion
    }
}
