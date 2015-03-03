namespace Unic.Flex.Website.Models.FlexSpeak
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The async tasks contract.
    /// </summary>
    [DataContract]
    public class AsyncTaks
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [DataMember(Name = "data")]
        public IEnumerable<AsyncTask> Data { get; set; }
    }
}