namespace Unic.Flex.Model.DomainModel.Plugs
{
    using Unic.Flex.Model.GlassExtensions.Attributes;

    /// <summary>
    /// Base class for all plugs.
    /// </summary>
    public abstract class PlugBase : ItemBase
    {
        //// todo: remove this message here and move it to the form -> 1 error message per form -> discuss this with Chris before implementing
        
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
