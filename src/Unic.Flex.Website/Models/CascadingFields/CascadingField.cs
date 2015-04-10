namespace Unic.Flex.Website.Models.CascadingFields
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// A cascading field
    /// </summary>
    [DataContract]
    public class CascadingField
    {
        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        [DataMember(Name = "options")]
        public virtual IList<CascadingOption> Options { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        [DataMember(Name = "errorMessage", EmitDefaultValue = false)]
        public virtual string ErrorMessage { get; set; }
    }
}