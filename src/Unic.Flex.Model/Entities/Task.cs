namespace Unic.Flex.Model.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Entity to store a specific task/plug.
    /// </summary>
    public class Task
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
        /// Gets or sets the job identifier.
        /// </summary>
        /// <value>
        /// The job identifier.
        /// </value>
        [Required]
        public virtual int JobId { get; set; }

        /// <summary>
        /// Gets or sets the item identifier.
        /// </summary>
        /// <value>
        /// The item identifier.
        /// </value>
        [Required]
        public virtual Guid ItemId { get; set; }

        /// <summary>
        /// Gets or sets the number of tries.
        /// </summary>
        /// <value>
        /// The number of tries.
        /// </value>
        [Required]
        public virtual int NumberOfTries { get; set; }

        /// <summary>
        /// Gets or sets the last try.
        /// </summary>
        /// <value>
        /// The last try.
        /// </value>
        [Required]
        public virtual DateTime LastTry { get; set; }

        /// <summary>
        /// Gets or sets the job.
        /// </summary>
        /// <value>
        /// The job.
        /// </value>
        [Required]
        public virtual Job Job { get; set; }
    }
}
