using Supido.Core.Utils;

namespace Supido.Business.Audit
{
    /// <summary>
    /// Types of Audit
    /// </summary>
    public enum AuditType
    {
        [StringValue("none")]
        None,
        [StringValue("simple")]
        Simple,
        [StringValue("delta")]
        Delta,
        [StringValue("full")]
        Full
    }
}
