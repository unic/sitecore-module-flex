namespace Unic.Flex.Model.DomainModel.Components
{
    /// <summary>
    /// View model properties for field dependency
    /// </summary>
    public interface IVisibilityDependencyViewModel
    {
        /// <summary>
        /// Gets or sets the dependent field.
        /// </summary>
        /// <value>
        /// The dependent field.
        /// </value>
        string DependentFrom { get; set; }

        /// <summary>
        /// Gets or sets the dependent value.
        /// </summary>
        /// <value>
        /// The dependent value.
        /// </value>
        string DependentValue { get; set; }
    }
}
