namespace Unic.Flex.Mapping
{
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.ViewModel.Forms;

    public interface IModelConverterService
    {
        IFormViewModel ConvertToViewModel(Form form);

        T GetViewModel<T>(object domainModel);
    }
}
