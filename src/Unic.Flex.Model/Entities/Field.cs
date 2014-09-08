namespace Unic.Flex.Model.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Field entity
    /// </summary>
    public class Field
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
        /// Gets or sets the session.
        /// </summary>
        /// <value>
        /// The session.
        /// </value>
        [Required]
        public virtual Session Session { get; set; }

        /// <summary>
        /// Gets or sets the item identifier.
        /// </summary>
        /// <value>
        /// The item identifier.
        /// </value>
        [Required]
        public virtual Guid ItemId { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [Required]
        public virtual string Value { get; set; }

        //// todo: add blob values for attachments
    }
}
