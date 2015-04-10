namespace Unic.Flex.Website.Models.FlexSpeak.GetAvailableForms
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The data wrapper
    /// </summary>
    [DataContract]
    public class DataWrapper
    {
        /// <summary>
        /// Gets or sets the data set.
        /// </summary>
        /// <value>
        /// The data set.
        /// </value>
        [DataMember(Name = "dataset")]
        public IEnumerable<DataSet> DataSet { get; set; }
    }
}