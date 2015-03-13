namespace Unic.Flex.Model.Plugs
{
    using System;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.Forms;

    /// <summary>
    /// Base class for all load plugs
    /// </summary>
    public abstract class SavePlugBase : ISavePlug
    {
        /// <summary>
        /// Gets or sets the item identifier.
        /// </summary>
        /// <value>
        /// The item identifier.
        /// </value>
        [SitecoreId]
        public virtual Guid ItemId { get; set; }
        
        /// <summary>
        /// Gets a value indicating whether this plug should be executed asynchronous.
        /// </summary>
        /// <value>
        /// <c>true</c> if this plug should be executed asynchronous; otherwise, <c>false</c>.
        /// </value>
        public abstract bool IsAsync { get; }

        /// <summary>
        /// Executes the save plug.
        /// </summary>
        /// <param name="form">The form.</param>
        public abstract void Execute(Form form);
    }
}
