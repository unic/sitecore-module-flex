namespace Unic.Flex.Model.ViewModel.Fields
{
    using System.Collections.Generic;
    using Unic.Flex.Model.DomainModel.Fields;
    using Unic.Flex.Model.Presentation;

    /// <summary>
    /// Generic field view model interface with generic value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public interface IFieldViewModel<TValue> : IFieldViewModel
    {
        /// <summary>
        /// Gets or sets the value. Property hides the <c>object</c> property from the non-generic base interface.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        new TValue Value { get; set; }
    }

    /// <summary>
    /// Field view model.
    /// </summary>
    public interface IFieldViewModel : IPresentationComponent
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        object Value { get; set; }

        /// <summary>
        /// Gets or sets the text value.
        /// </summary>
        /// <value>
        /// The text value.
        /// </value>
        string TextValue { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        string Key { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        string Label { get; set; }

        /// <summary>
        /// Gets or sets the CSS class.
        /// </summary>
        /// <value>
        /// The CSS class.
        /// </value>
        string CssClass { get; set; }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        IDictionary<string, object> Attributes { get; }

        /// <summary>
        /// Binds the needed attributes and properties after converting from domain model to the view model
        /// </summary>
        void BindProperties();

        /// <summary>
        /// Adds a CSS class.
        /// </summary>
        /// <param name="cssClass">The CSS class.</param>
        void AddCssClass(string cssClass);
    }
}
