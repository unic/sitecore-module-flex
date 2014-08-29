namespace Unic.Flex.Model.DomainModel.Fields
{
    using System;
    using System.Collections.Generic;
    using Unic.Flex.Model.DomainModel.Components;
    using Unic.Flex.Model.DomainModel.Global;
    using Unic.Flex.Model.Validation;

    /// <summary>
    /// Generic interface for a field.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public interface IField<TValue> : IField
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        new TValue Value { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        new TValue DefaultValue { get; set; }
    }

    /// <summary>
    /// Non-generic interface for a field.
    /// </summary>
    public interface IField : ITooltip
    {
        /// <summary>
        /// Gets or sets the item identifier.
        /// </summary>
        /// <value>
        /// The item identifier.
        /// </value>
        Guid ItemId { get; set; }
        
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        object Value { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        object DefaultValue { get; set; }

        /// <summary>
        /// Gets the text value.
        /// </summary>
        /// <value>
        /// The text value.
        /// </value>
        string TextValue { get; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        string Key { get; }

        /// <summary>
        /// Gets or sets the custom CSS class.
        /// </summary>
        /// <value>
        /// The custom CSS class.
        /// </value>
        Specification CustomCssClass { get; set; }

        /// <summary>
        /// Gets or sets the additional CSS class.
        /// </summary>
        /// <value>
        /// The additional CSS class.
        /// </value>
        string AdditionalCssClass { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is required.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is required; otherwise, <c>false</c>.
        /// </value>
        bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message.
        /// </value>
        string ValidationMessage { get; set; }

        /// <summary>
        /// Gets or sets the validators.
        /// </summary>
        /// <value>
        /// The validators.
        /// </value>
        IEnumerable<IValidator> Validators { get; set; }

        /// <summary>
        /// Gets the default validators.
        /// </summary>
        /// <value>
        /// The default validators.
        /// </value>
        IList<IValidator> DefaultValidators { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to show this field in the summary or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this field should be shown in the summary; otherwise, <c>false</c>.
        /// </value>
        bool ShowInSummary { get; set; }
    }
}
