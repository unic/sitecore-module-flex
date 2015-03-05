namespace Unic.Flex.Model.ViewModel.Components
{
    /// <summary>
    /// View model properties for field dependency
    /// </summary>
    public interface IVisibilityDependencyViewModel
    {
        /// <summary>
        /// Gets or sets the dependent field identifier.
        /// </summary>
        /// <value>
        /// The dependent field identifier.
        /// </value>
        string DependentFieldId { get; set; }

        /// <summary>
        /// Gets or sets the dependent value.
        /// </summary>
        /// <value>
        /// The dependent value.
        /// </value>
        string DependentValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is hidden.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is hidden; otherwise, <c>false</c>.
        /// </value>
        bool IsHidden { get; set; }
    }
}
