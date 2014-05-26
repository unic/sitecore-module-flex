namespace Unic.Flex.Model.ViewModel.Fields
{
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
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        string Key { get; set; }
    }
}
