namespace Unic.Flex.Model.Configuration
{
    using Unic.Configuration;

    /// <summary>
    /// Globalization configuration
    /// </summary>
    public class GlobalizationConfiguration : AbstractConfiguration
    {
        /// <summary>
        /// Gets or sets the date format.
        /// </summary>
        /// <value>
        /// The date format.
        /// </value>
        [Configuration(FieldName = "Date Format")]
        public virtual string DateFormat { get; set; }
    }
}
