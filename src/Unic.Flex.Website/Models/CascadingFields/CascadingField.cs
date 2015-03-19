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
        public IEnumerable<CascadingOption> Options { get; set; }
    }
}