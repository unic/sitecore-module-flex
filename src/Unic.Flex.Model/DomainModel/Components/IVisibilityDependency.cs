namespace Unic.Flex.Model.DomainModel.Components
{
    using Unic.Flex.Model.DomainModel.Fields;

    /// <summary>
    /// Interface for components which possible have some field dependendies.
    /// </summary>
    public interface IVisibilityDependency
    {
        /// <summary>
        /// Gets or sets the dependent field.
        /// </summary>
        /// <value>
        /// The dependent field.
        /// </value>
        IField DependentField { get; set; }

        /// <summary>
        /// Gets or sets the dependent value.
        /// </summary>
        /// <value>
        /// The dependent value.
        /// </value>
        string DependentValue { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is hidden.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is hidden; otherwise, <c>false</c>.
        /// </value>
        bool IsHidden { get; }
    }
}
