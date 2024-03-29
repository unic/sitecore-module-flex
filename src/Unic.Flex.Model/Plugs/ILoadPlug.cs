﻿namespace Unic.Flex.Model.Plugs
{
    using System;
    using Unic.Flex.Model.Forms;

    /// <summary>
    /// Interface for defining a load plug
    /// </summary>
    public interface ILoadPlug
    {
        /// <summary>
        /// Gets or sets the item identifier.
        /// </summary>
        /// <value>
        /// The item identifier.
        /// </value>
        Guid ItemId { get; set; }
        
        /// <summary>
        /// Executes the load plug.
        /// </summary>
        /// <param name="form">The form.</param>
        void Execute(IForm form);

        bool IgnoreHttpMethodExecutionFilter { get; set; }
    }
}
