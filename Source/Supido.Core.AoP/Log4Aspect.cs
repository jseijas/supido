using log4net;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using System;
using System.Diagnostics;
using System.Text;

namespace Supido.Core.AoP
{
    /// <summary>
    /// Aspect for automatic Log of methods.
    /// </summary>
    [Serializable]
    [MulticastAttributeUsage(MulticastTargets.Method)]
    public class LogAspect : OnMethodBoundaryAspect
    {
        #region - Fields -

        /// <summary>
        /// Indicates if the aspect must log the arguments of the method call.
        /// </summary>
        private bool showParameters = true;

        /// <summary>
        /// Indicates if the aspect must log the execution time of the method call.
        /// </summary>
        private bool showTime = true;

        /// <summary>
        /// The stop watch in order to measuring execution time.
        /// </summary>
        [NonSerialized]
        private Stopwatch stopWatch;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets or sets a value indicating whether [show parameters].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show parameters]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowParameters
        {
            get { return this.showParameters; }
            set { this.showParameters = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show time].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show time]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowTime
        {
            get { return this.showTime; }
            set { this.showTime = value; }
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Method executed <b>before</b> the body of methods to which this aspect is applied.
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed, which are its arguments, and how should the execution continue
        /// after the execution of <see cref="M:PostSharp.Aspects.IOnMethodBoundaryAspect.OnEntry(PostSharp.Aspects.MethodExecutionArgs)" />.</param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            ILog log = LogManager.GetLogger(args.Method.DeclaringType);
            if (this.showTime)
            {
                this.stopWatch = Stopwatch.StartNew();
            }
            if ((this.ShowParameters) && (args.Arguments.Count > 0))
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < args.Arguments.Count; i++)
                {
                    object obj = args.Arguments[i];
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    if (obj == null)
                    {
                        sb.Append("null");
                    }
                    else if (obj is string)
                    {
                        sb.Append("\"");
                        sb.Append((string)obj);
                        sb.Append("\"");
                    }
                    else
                    {
                        sb.Append(obj.ToString());
                    }
                }
                log.DebugFormat("Entering [{0}] with arguments [({1})]", args.Method.Name, sb.ToString());
            }
            else
            {
                log.DebugFormat("Entering [{0}]", args.Method.Name);
            }
            base.OnEntry(args);
        }

        /// <summary>
        /// Method executed <b>after</b> the body of methods to which this aspect is applied,
        /// even when the method exists with an exception (this method is invoked from
        /// the <c>finally</c> block).
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed and which are its arguments.</param>
        public override void OnExit(MethodExecutionArgs args)
        {
            ILog log = LogManager.GetLogger(args.Method.DeclaringType);
            if (this.showTime)
            {
                this.stopWatch.Stop();
                log.DebugFormat("Leaving [{0}]. Tooks [{1}]ms to execute", args.Method.Name, stopWatch.ElapsedMilliseconds);
            }
            else
            {
                log.DebugFormat("Leaving [{0}]", args.Method.Name);
            }
            base.OnExit(args);
        }

        /// <summary>
        /// Method executed <b>after</b> the body of methods to which this aspect is applied,
        /// in case that the method resulted with an exception.
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed and which are its arguments.</param>
        public override void OnException(MethodExecutionArgs args)
        {
            ILog log = LogManager.GetLogger(args.Method.DeclaringType);
            log.WarnFormat("Exception executing at [{0}]: Exception is [{1}] with Message [{2}]", args.Method.Name, args.Exception.GetType().Name, args.Exception.Message);
            base.OnException(args);
        }

        #endregion
    }
}
