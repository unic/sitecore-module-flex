namespace Unic.Flex.Website.Models.CascadingFields
{
    using System.Runtime.Serialization;

    /// <summary>
    /// A cascading option
    /// </summary>
    [DataContract]
    public class CascadingOption
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        [DataMember(Name = "text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}