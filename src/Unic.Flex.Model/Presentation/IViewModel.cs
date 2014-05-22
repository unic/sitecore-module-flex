namespace Unic.Flex.Model.Presentation
{
    using Unic.Flex.Model;
    using Unic.Flex.Model.DomainModel;

    /// <summary>
    /// Interface for all view models which stores a reference to the corresponding domain model object.
    /// </summary>
    public interface IViewModel
    {
        /// <summary>
        /// Gets or sets the domain model.
        /// </summary>
        /// <value>
        /// The domain model.
        /// </value>
        ItemBase DomainModel { get; set; }
    }
}
