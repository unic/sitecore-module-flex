namespace Unic.Flex.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Entity for a job, which should execute several plugs (tasks)
    /// </summary>
    public class Job
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
        /// Gets or sets the item identifier.
        /// </summary>
        /// <value>
        /// The item identifier.
        /// </value>
        [Required]
        public virtual Guid ItemId { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [Required]
        public virtual string Data { get; set; }

        /// <summary>
        /// Gets or sets the job origin.
        /// </summary>
        /// <value>
        /// The origin.
        /// </value>
        public virtual string JobOrigin { get; set; }

        /// <summary>
        /// Gets or sets the tasks.
        /// </summary>
        /// <value>
        /// The tasks.
        /// </value>
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
