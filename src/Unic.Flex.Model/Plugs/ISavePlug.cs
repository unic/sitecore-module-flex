﻿namespace Unic.Flex.Model.Plugs
{
    using Sitecore.Rules;
    using System;
    using Unic.Flex.Model.Forms;

    /// <summary>
    /// Interface for defining a save plug
    /// </summary>
    public interface ISavePlug
    {
        /// <summary>
        /// Gets or sets the item identifier.
        /// </summary>
        /// <value>
        /// The item identifier.
        /// </value>
        Guid ItemId { get; set; }
        
        /// <summary>
        /// Gets a value indicating whether this plug should be executed asynchronous.
        /// </summary>
        /// <value>
        /// <c>true</c> if this plug should be executed asynchronous; otherwise, <c>false</c>.
        /// </value>
        bool IsAsync { get; }
        
        RuleList<RuleContext> ConditionalRule { get; set; }

        string TaskData { get; set; }

        /// <summary>
        /// Executes the load plug.
        /// </summary>
        /// <param name="form">The form.</param>
        void Execute(IForm form);

        /// <summary>
        /// Provides relevant data for async plug execution
        /// </summary>
        /// <returns></returns>
        string GetTaskDataForStorage(IForm form);
    }
}
