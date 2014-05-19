namespace Unic.Flex.Context
{
    using System.Linq;
    using Glass.Mapper.Sc;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Unic.Flex.Mapping;
    using Unic.Flex.Model.Forms;
    using Unic.Flex.Model.Sections;
    using Unic.Flex.Model.Steps;

    public class ContextService : IContextService
    {
        private readonly IFormRepository formRepository;

        private readonly IUserDataRepository userDataRepository;

        public ContextService(IFormRepository formRepository, IUserDataRepository userDataRepository)
        {
            this.formRepository = formRepository;
            this.userDataRepository = userDataRepository;
        }

        public Form LoadForm(string dataSource, ISitecoreContext sitecoreContext)
        {
            return this.formRepository.LoadForm(dataSource, sitecoreContext);
        }

        public void PopulateFormValues(Form form)
        {
            Assert.ArgumentNotNull(form, "form");
            
            foreach (var stepSection in form.Steps.OfType<StandardStep>().SelectMany(step => step.Sections))
            {
                var reusableSection = stepSection as ReusableSection;
                var section = reusableSection != null ? reusableSection.Section : stepSection as StandardSection;
                foreach (var field in section.Fields)
                {
                    field.Value = this.userDataRepository.GetValue(form.ItemId.ToString(), field.ItemId.ToString());
                }
            }
        }

        public void StoreFormValues(Form form, FormViewModel viewModel)
        {
            Assert.ArgumentNotNull(form, "form");
            Assert.ArgumentNotNull(viewModel, "viewModel");

            foreach (var field in viewModel.Step.Sections.SelectMany(section => section.Fields))
            {
                this.userDataRepository.SetValue(form.ItemId.ToString(), field.Key, field.Value);
            }
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
