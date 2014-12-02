namespace Unic.Flex.Model.Configuration
{
    using Unic.Configuration;
    using Unic.Flex.Model.DomainModel.Global;

    /// <summary>
    /// Configuration model für presentation config.
    /// </summary>
    public class PresentationConfiguration : AbstractConfiguration
    {
        /// <summary>
        /// Gets or sets the theme.
        /// </summary>
        /// <value>
        /// The theme.
        /// </value>
        [Configuration(FieldName = "Theme")]
        public virtual Specification Theme { get; set; }
    }
}
