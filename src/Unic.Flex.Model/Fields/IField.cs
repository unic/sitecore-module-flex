namespace Unic.Flex.Model.Fields
{
    using System;
    using System.Collections.Generic;
    using Glass.Mapper.Sc.Fields;
    using Unic.Flex.Model.DomainModel.Components;
    using Unic.Flex.Model.Presentation;
    using Unic.Flex.Model.Specifications;
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
    public interface IField : ITooltip, IVisibilityDependency, IValidatableObject, IPresentationComponent, IReusableComponent<IField>
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        string Id { get; }
        
        /// <summary>
        /// Gets or sets the item identifier.
        /// </summary>
        /// <value>
        /// The item identifier.
        /// </value>
        Guid ItemId { get; set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        Type Type { get; }
        
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
        /// Gets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        string Label { get; }

        /// <summary>
        /// Gets or sets the label addition.
        /// </summary>
        /// <value>
        /// The label addition.
        /// </value>
        string LabelAddition { get; set; }

        /// <summary>
        /// Gets the text label.
        /// </summary>
        /// <value>
        /// The text label.
        /// </value>
        string TextLabel { get; }

        /// <summary>
        /// Gets the label link.
        /// </summary>
        /// <value>
        /// The label link.
        /// </value>
        Link LabelLink { get; }

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
        IEnumerable<IValidator> Validators { get; }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        IDictionary<string, object> Attributes { get; }

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

        /// <summary>
        /// Adds the CSS class.
        /// </summary>
        /// <param name="cssClass">The CSS class.</param>
        void AddCssClass(string cssClass);

        /// <summary>
        /// Binds the properties.
        /// </summary>
        void BindProperties();
    }
}
