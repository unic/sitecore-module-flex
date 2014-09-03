namespace Unic.Flex.Model.DomainModel.Plugs
{
    using Unic.Flex.Model.GlassExtensions.Attributes;

    /// <summary>
    /// Base class for all plugs.
    /// </summary>
    public abstract class PlugBase : ItemBase
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        [SitecoreDictionaryFallbackField("Error Message", "Default plug error")]
        public virtual string ErrorMessage { get; set; }
    }
}
