
namespace Supido.Templates.Parsing.Evaluators
{
    /// <summary>
    /// Class for a json object that is a javascript function.
    /// </summary>
    public class JSFunction : JsonObject
    {
        #region - Fields -

        /// <summary>
        /// The function count, for given name to anonymous functions.
        /// </summary>
        public static int FuncCount = 0;

        /// <summary>
        /// The root variable node.
        /// </summary>
        private VariableNode root;

        /// <summary>
        /// The token
        /// </summary>
        private Token token;

        /// <summary>
        /// The parameters
        /// </summary>
        private Token parameters;

        /// <summary>
        /// The body
        /// </summary>
        private Token body;

        /// <summary>
        /// The name
        /// </summary>
        private string name;

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="JSFunction"/> class.
        /// </summary>
        public JSFunction()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JSFunction"/> class.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="token">The token.</param>
        public JSFunction(VariableNode root, Token token)
        {
            this.root = root;
            this.token = token;
            if (token.NodeCount == 3)
            {
                // named function
                this.name = token[0].Text;
                this.parameters = token[1];
                this.body = token[2];
            }
            else
            {
                // Anonymous function.
                this.name = string.Format("_anonymous_{0}", FuncCount++);
                this.parameters = token[0];
                this.body = token[1];
            }
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Apply the function in the evaluator for executing. Hey dude! If you don't found any reference to this function, don't delete it! Is called using dynamic.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="self">The self.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public virtual dynamic Apply(JavascriptEvaluator e, dynamic self, params dynamic[] args)
        {
            var originalContext = e.environment;
            var originalReturningState = e.isReturning;
            dynamic result = null;

            try
            {
                e.environment = root.AddVariable("this", self);
                e.isReturning = false;
                int i = 0;
                foreach (var p in parameters.Tokens)
                    e.AddVariable(p.Text, args[i++]);
                e.Eval(body);
                result = e.result;
            }
            finally
            {
                e.result = null;
                e.environment = originalContext;
                e.isReturning = originalReturningState;
            }
            return result;
        }

        #endregion
    }

}
