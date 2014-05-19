namespace Unic.Flex.Context
{
    using Glass.Mapper.Sc;
    using Sitecore.Data.Items;
    using Unic.Flex.Model.Forms;

    public interface IContextService
    {
        Form LoadForm(string dataSource, ISitecoreContext sitecoreContext);

        void PopulateFormValues(Form form);

        void StoreFormValues(Form form, FormViewModel viewModel);

        string GetRenderingDatasource(Item item, DeviceItem device);
    }
}
