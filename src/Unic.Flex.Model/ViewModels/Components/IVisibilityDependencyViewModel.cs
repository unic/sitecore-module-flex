namespace Unic.Flex.Model.ViewModels.Components
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
        // todo: remote all view model classes not needed anymore
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
