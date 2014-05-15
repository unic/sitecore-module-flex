namespace Unic.Flex.Context
{
    using System.Linq;
    using Glass.Mapper.Sc;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Unic.Flex.DomainModel.Forms;
    using Unic.Flex.Mapping;

    public class ContextService : IContextService
    {
        private readonly IFormRepository formRepository;

        public ContextService(IFormRepository formRepository)
        {
            this.formRepository = formRepository;
        }

        public Form LoadForm(string dataSource, ISitecoreContext sitecoreContext)
        {
            return this.formRepository.LoadForm(dataSource, sitecoreContext);
        }

        public string GetRenderingDatasource(Item item, DeviceItem device)
        {
            // todo: move id to the config file
            
            var renderingReferences = item.Visualization.GetRenderings(device, false);
            var rendering = renderingReferences.FirstOrDefault(reference => reference.RenderingID == new ID("{FA6D13FC-4415-4403-A412-1EB8E2045DF9}"));
            return rendering == null ? string.Empty : rendering.Settings.DataSource;
        }
    }
}
