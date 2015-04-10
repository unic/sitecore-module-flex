namespace Unic.Flex.Model.Components
{
    /// <summary>
    /// Interface for cascading fields
    /// </summary>
    public interface ICascadingDependency : IFieldDependency
    {
        /// <summary>
        /// Gets or sets a value indicating whether this field is a cascading field.
        /// </summary>
        /// <value>
        /// <c>true</c> if this field is a cascading field; otherwise, <c>false</c>.
        /// </value>
        bool IsCascadingField { get; set; }
    }
}
