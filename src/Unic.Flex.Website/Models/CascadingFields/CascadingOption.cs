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
        public virtual string Text { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [DataMember(Name = "value")]
        public virtual string Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CascadingOption"/> is selected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if selected; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "selected")]
        public virtual bool Selected { get; set; }
    }
}