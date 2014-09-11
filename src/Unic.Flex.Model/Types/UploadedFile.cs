namespace Unic.Flex.Model.Types
{
    /// <summary>
    /// A wrapper type for an uploaded file
    /// </summary>
    public class UploadedFile
    {
        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        public virtual string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the length of the content.
        /// </summary>
        /// <value>
        /// The length of the content.
        /// </value>
        public virtual int ContentLength { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public virtual string FileName { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public virtual byte[] Data { get; set; }
    }
}
