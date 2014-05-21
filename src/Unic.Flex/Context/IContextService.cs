namespace Unic.Flex.Context
{
    using Glass.Mapper.Sc;
    using Sitecore.Data.Items;
    using Unic.Flex.Model.Forms;

    /// <summary>
    /// Service containing context based business logic.
    /// </summary>
    public interface IContextService
    {
        /// <summary>
        /// Loads the form based on a datasource.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <param name="sitecoreContext">The sitecore context.</param>
        /// <returns>The loaded form domain model object.</returns>
        Form LoadForm(string dataSource, ISitecoreContext sitecoreContext);

        /// <summary>
        /// Populates the form values from the session into the form.
        /// </summary>
        /// <param name="form">The form domain model.</param>
        void PopulateFormValues(Form form);

        /// <summary>
        /// Stores the form values into the session.
        /// </summary>
        /// <param name="form">The form domain model.</param>
        /// <param name="viewModel">The form view model containing the current values.</param>
        void StoreFormValues(Form form, FormViewModel viewModel);

        /// <summary>
        /// Gets the rendering datasource of a form.
        /// </summary>
        /// <param name="item">The item to search for a referenced form.</param>
        /// <param name="device">The device.</param>
        /// <returns>Datasource/form id if form is included on item</returns>
        string GetRenderingDatasource(Item item, DeviceItem device);
    }
}
