
namespace Supido.Core.Container
{
    /// <summary>
    /// Lifetime of the inversion items.
    /// </summary>
    public enum IoCLifetime
    {
        /// <summary>
        /// The inversion item is a singleton.
        /// </summary>
        Singleton,

        /// <summary>
        /// The inversion item creates a new instance per call.
        /// </summary>
        PerCall,

        /// <summary>
        /// The inversion item creates instances per token.
        /// </summary>
        PerToken
    }
}
