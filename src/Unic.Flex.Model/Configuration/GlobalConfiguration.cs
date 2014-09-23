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

        /// <summary>
        /// Gets or sets a value indicating whether async plug execution is allowed.
        /// </summary>
        /// <value>
        /// <c>true</c> if async plug execution is allowed; otherwise, <c>false</c>.
        /// </value>
        [Configuration(FieldName = "Is Async Execution Allowed")]
        public bool IsAsyncExecutionAllowed { get; set; }

        /// <summary>
        /// Gets or sets the maximum retries.
        /// </summary>
        /// <value>
        /// The maximum retries.
        /// </value>
        [Configuration(FieldName = "Max Retries")]
        public int MaxRetries { get; set; }
    }
}
