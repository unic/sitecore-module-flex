namespace Unic.Flex.Context
{
    using System;
    using System.Linq;
    using Glass.Mapper.Sc;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Shell.Framework.Commands.Masters;
    using Unic.Flex.Mapping;
    using Unic.Flex.Model.DomainModel.Fields;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Sections;
    using Unic.Flex.Model.DomainModel.Steps;
    using Unic.Flex.Model.ViewModel.Forms;

    /// <summary>
    /// The service containing contexxt based business logic.
    /// </summary>
    public class ContextService : IContextService
    {
        /// <summary>
        /// The form repository
        /// </summary>
        private readonly IFormRepository formRepository;

        /// <summary>
        /// The user data repository
        /// </summary>
        private readonly IUserDataRepository userDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextService"/> class.
        /// </summary>
        /// <param name="formRepository">The form repository.</param>
        /// <param name="userDataRepository">The user data repository.</param>
        public ContextService(IFormRepository formRepository, IUserDataRepository userDataRepository)
        {
            this.formRepository = formRepository;
            this.userDataRepository = userDataRepository;
        }

        /// <summary>
        /// Loads the form based on a datasource.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <param name="sitecoreContext">The sitecore context.</param>
        /// <returns>
        /// The loaded form domain model object.
        /// </returns>
        public Form LoadForm(string dataSource, ISitecoreContext sitecoreContext)
        {
            return this.formRepository.LoadForm(dataSource, sitecoreContext);
        }

        /// <summary>
        /// Populates the form values from the session into the form.
        /// </summary>
        /// <param name="form">The form domain model.</param>
        public void PopulateFormValues(Form form)
        {
            Assert.ArgumentNotNull(form, "form");
            
            foreach (var stepSection in form.Steps.SelectMany(step => step.Sections))
            {
                var reusableSection = stepSection as ReusableSection;
                var section = reusableSection != null ? reusableSection.Section : stepSection as StandardSection;
                foreach (var field in section.Fields)
                {
                    // todo: this must be generic, not statically a string
                    (field as FieldBase<string>).Value = this.userDataRepository.GetValue(form.ItemId.ToString(), field.ItemId.ToString()) as string;
                }
            }
        }

        /// <summary>
        /// Stores the form values into the session.
        /// </summary>
        /// <param name="form">The form domain model.</param>
        /// <param name="viewModel">The form view model containing the current values.</param>
        public void StoreFormValues(Form form, FormViewModel viewModel)
        {
            Assert.ArgumentNotNull(form, "form");
            Assert.ArgumentNotNull(viewModel, "viewModel");

            foreach (var field in viewModel.Step.Sections.SelectMany(section => section.Fields))
            {
                this.userDataRepository.SetValue(form.ItemId.ToString(), field.Key, field.Value);
            }
        }

        /// <summary>
        /// Gets the rendering datasource of a form.
        /// </summary>
        /// <param name="item">The item to search for a referenced form.</param>
        /// <param name="device">The device.</param>
        /// <returns>
        /// Datasource/form id if form is included on item
        /// </returns>
        public string GetRenderingDatasource(Item item, DeviceItem device)
        {
            Assert.ArgumentNotNull(item, "item");
            Assert.ArgumentNotNull(device, "device");
            
            // todo: move id to the config file
            
            var renderingReferences = item.Visualization.GetRenderings(device, false);
            var rendering = renderingReferences.FirstOrDefault(reference => reference.RenderingID == new ID("{FA6D13FC-4415-4403-A412-1EB8E2045DF9}"));
            return rendering == null ? string.Empty : rendering.Settings.DataSource;
        }
    }
}
