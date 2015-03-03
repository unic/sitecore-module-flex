namespace Unic.Flex.Website.Models.FlexSpeak.GetAvailableForms
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The data set
    /// </summary>
    [DataContract]
    public class DataSet
    {
        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        /// <value>
        /// The rows.
        /// </value>
        [DataMember(Name = "data")]
        public IEnumerable<DataRow> Rows { get; set; } 
    }
}