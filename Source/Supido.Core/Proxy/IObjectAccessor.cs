
namespace Supido.Core.Proxy
{
    /// <summary>
    /// Interface for accessing values of an instance by name.
    /// </summary>
    public interface IObjectAccessor
    {
        #region - Methods -

        /// <summary>
        /// Gets the value of a property of the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns></returns>
        object GetValue(object instance, string name);

        /// <summary>
        /// Sets the value of a property of the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value.</param>
        void SetValue(object instance, string name, object value);

        #endregion
    }
}
