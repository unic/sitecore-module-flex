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
        /// Initializes a new instance of the <see cref="AsyncTaks"/> class.
        /// </summary>
        public AsyncTaks()
        {
            this.Data = new List<AsyncTask>();
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [DataMember(Name = "data", EmitDefaultValue = false)]
        public IList<AsyncTask> Data { get; private set; }
    }
}