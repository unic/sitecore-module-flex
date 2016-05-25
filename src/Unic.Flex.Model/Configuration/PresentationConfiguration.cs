namespace Unic.Flex.Model.Configuration
{
    using Unic.Configuration.Core;
    using Unic.Flex.Model.Specifications;

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
