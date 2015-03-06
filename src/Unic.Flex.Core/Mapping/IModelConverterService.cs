namespace Unic.Flex.Core.Mapping
{
    using Unic.Flex.Model.Forms;
    using Unic.Flex.Model.ViewModel.Forms;

    /// <summary>
    /// Converting methods between domain and view model
    /// </summary>
    public interface IModelConverterService
    {
        /// <summary>
        /// Converts the domain model to view model.
        /// </summary>
        /// <param name="form">The form domain model.</param>
        /// <returns>The view model</returns>
        IFormViewModel ConvertToViewModel(Form form);

        /// <summary>
        /// Gets a view model instance base on the domain model.
        /// The view model must have the same class name as the domain model with the postfix "ViewModel".
        /// Both classes have to be in the same assembly.
        /// </summary>
        /// <typeparam name="T">Interface type of the desired view model</typeparam>
        /// <param name="domainModel">The domain model.</param>
        /// <returns>A newly created view model, if class was found.</returns>
        T GetViewModel<T>(object domainModel);
    }
}
