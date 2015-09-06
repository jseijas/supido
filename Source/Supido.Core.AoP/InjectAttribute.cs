using PostSharp.Aspects;
using Supido.Core.Container;
using Supido.Core.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supido.Core.AoP
{
    /// <summary>
    /// Attribute Inject for property dependency injection.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class InjectAttribute : LocationInterceptionAspect
    {

        public string Token { get; set; }

        public string TokenProperty { get; set; }

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
                if (string.IsNullOrEmpty(this.Token))
                {
                    if (string.IsNullOrEmpty(this.TokenProperty))
                    {
                        args.Value = IoC.Get(args.Location.LocationType);
                    }
                    else
                    {
                        object value = ObjectProxyFactory.Get(args.Instance).GetValue(args.Instance, this.TokenProperty);
                        if (value == null)
                        {
                            args.Value = IoC.Get(args.Location.LocationType);
                        }
                        else
                        {
                            args.Value = IoC.Get(args.Location.LocationType, value.ToString());
                        }
                    }
                }
                else
                {
                    args.Value = IoC.Get(args.Location.LocationType, this.Token);
                }
                args.ProceedSetValue();
            }
        }
    }
}
