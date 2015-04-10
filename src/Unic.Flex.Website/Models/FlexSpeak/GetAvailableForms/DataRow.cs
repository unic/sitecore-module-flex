namespace Unic.Flex.Website.Models.FlexSpeak.GetAvailableForms
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The data row
    /// </summary>
    [DataContract]
    public class DataRow
    {
        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        [DataMember(Name = "repository")]
        public string Repository { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        [DataMember(Name = "lang")]
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the forms.
        /// </summary>
        /// <value>
        /// The forms.
        /// </value>
        [DataMember(Name = "forms")]
        public int Forms { get; set; }
    }
}