namespace Unic.Flex.Model.Entities
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Entity for storing a file in the database.
    /// </summary>
    public class File
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Required]
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        [Required]
        public virtual string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the length of the content.
        /// </summary>
        /// <value>
        /// The length of the content.
        /// </value>
        [Required]
        public virtual int ContentLength { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        [Required]
        public virtual string FileName { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [Required]
        public virtual byte[] Data { get; set; }
    }
}
