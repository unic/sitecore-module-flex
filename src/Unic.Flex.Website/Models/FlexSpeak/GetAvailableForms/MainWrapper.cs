namespace Unic.Flex.Website.Models.FlexSpeak.GetAvailableForms
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The main wrapper
    /// </summary>
    [DataContract]
    public class MainWrapper
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [DataMember(Name = "data")]
        public DataWrapper Data { get; set; }
    }
}