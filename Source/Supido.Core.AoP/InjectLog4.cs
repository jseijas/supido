using log4net;
using PostSharp.Aspects;
using System;

namespace Supido.Core.AoP
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class InjectLog : LocationInterceptionAspect
    {
        /// <summary>
        /// Method invoked <i>instead</i> of the <c>Get</c> semantic of the field or property to which the current aspect is applied,
        /// i.e. when the value of this field or property is retrieved.
        /// </summary>
        /// <param name="args">Advice arguments.</param>
        public override void OnGetValue(LocationInterceptionArgs args)
        {
            args.ProceedGetValue();
            if (args.Value == null)
            {
                args.Value = LogManager.GetLogger(args.Location.DeclaringType);
                args.ProceedSetValue();
            }
        }
    }
}
