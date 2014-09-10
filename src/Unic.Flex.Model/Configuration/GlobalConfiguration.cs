namespace Unic.Flex.Model.Configuration
{
    using Unic.Configuration;

    /// <summary>
    /// The global configuration
    /// </summary>
    public class GlobalConfiguration : AbstractConfiguration
    {
        /// <summary>
        /// Gets or sets the optional fields label text.
        /// </summary>
        /// <value>
        /// The optional fields label text.
        /// </value>
        [Configuration(FieldName = "Optional Fields Label Text")]
        public string OptionalFieldsLabelText { get; set; }
    }
}
