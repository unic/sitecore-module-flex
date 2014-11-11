namespace Unic.Flex.Context
{
    using System.Collections.Generic;
    using Glass.Mapper.Sc;
    using Sitecore.Data.Items;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Steps;
    using Unic.Flex.Model.ViewModel.Forms;

    /// <summary>
    /// Service containing context based business logic.
    /// </summary>
    public interface IContextService
    {
        /// <summary>
        /// Loads the form based on a datasource.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <returns>
        /// The loaded form domain model object.
        /// </returns>
        Form LoadForm(string dataSource);

        /// <summary>
        /// Populates the form values from the session into the form.
        /// </summary>
        /// <param name="form">The form domain model.</param>
        void PopulateFormValues(Form form);

        /// <summary>
        /// Populates the form values from a dictionary.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="values">The values.</param>
        void PopulateFormValues(Form form, IDictionary<string, object> values);

        /// <summary>
        /// Stores the form values into the session.
        /// </summary>
        /// <param name="viewModel">The form view model containing the current values.</param>
        void StoreFormValues(IFormViewModel viewModel);

        /// <summary>
        /// Determines whether the given step can be actually accessed. This is only valid if all previous steps has been processed.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="step">The step.</param>
        /// <returns>Boolean value if the step may accessed by the user or not</returns>
        bool IsStepAccessible(Form form, StepBase step);

        /// <summary>
        /// Gets the rendering datasource of a form.
        /// </summary>
        /// <param name="item">The item to search for a referenced form.</param>
        /// <param name="device">The device.</param>
        /// <returns>Datasource/form id if form is included on item</returns>
        string GetRenderingDatasource(Item item, DeviceItem device);
    }
}
