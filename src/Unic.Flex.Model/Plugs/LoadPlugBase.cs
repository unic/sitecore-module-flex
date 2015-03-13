namespace Unic.Flex.Model.Plugs
{
    using System;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.Forms;

    /// <summary>
    /// Base class for all load plugs
    /// </summary>
    public abstract class LoadPlugBase : ILoadPlug
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
        /// Executes the load plug.
        /// </summary>
        /// <param name="form">The form.</param>
        public abstract void Execute(Form form);
    }
}
