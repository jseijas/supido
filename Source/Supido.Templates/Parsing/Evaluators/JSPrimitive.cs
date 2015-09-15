using System;

namespace Supido.Templates.Parsing.Evaluators
{
    /// <summary>
    /// Class for a javascript primitive.
    /// </summary>
    public class JSPrimitive : JSFunction
    {
        #region - Fields -

        /// <summary>
        /// The function
        /// </summary>
        private Func<dynamic, dynamic[], dynamic> func;

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="JSPrimitive"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public JSPrimitive(Action<dynamic[]> action)
        {
            this.func = (self, args) =>
            {
                action(args);
                return null;
            };
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
        public override dynamic Apply(JavascriptEvaluator e, dynamic self, params dynamic[] args)
        {
            return func(self, args);
        }

        #endregion
    }
}
