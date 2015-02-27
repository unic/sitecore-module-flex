namespace Unic.Flex.Website.Models.FlexSpeak
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// An async task contract.
    /// </summary>
    [DataContract]
    public class AsyncTask
    {
        /// <summary>
        /// Gets or sets the task identifier.
        /// </summary>
        /// <value>
        /// The task identifier.
        /// </value>
        [DataMember(Name = "task_id", EmitDefaultValue = false)]
        public long TaskId { get; set; }
        
        /// <summary>
        /// Gets or sets the form.
        /// </summary>
        /// <value>
        /// The form.
        /// </value>
        [DataMember(Name = "form", EmitDefaultValue = false)]
        public string Form { get; set; }

        /// <summary>
        /// Gets or sets the plug.
        /// </summary>
        /// <value>
        /// The plug.
        /// </value>
        [DataMember(Name = "plug", EmitDefaultValue = false)]
        public string Plug { get; set; }

        /// <summary>
        /// Gets or sets the attemps.
        /// </summary>
        /// <value>
        /// The attemps.
        /// </value>
        [DataMember(Name = "attemps", EmitDefaultValue = false)]
        public int Attemps { get; set; }

        /// <summary>
        /// Gets the last try formatted.
        /// </summary>
        /// <value>
        /// The last try formatted.
        /// </value>
        [DataMember(Name = "last_try", EmitDefaultValue = false)]
        public string LastTryFormatted
        {
            get
            {
                return this.LastTry.ToString("G");
            }
        }

        /// <summary>
        /// Gets or sets the last try.
        /// </summary>
        /// <value>
        /// The last try.
        /// </value>
        public DateTime LastTry { get; set; }
    }
}