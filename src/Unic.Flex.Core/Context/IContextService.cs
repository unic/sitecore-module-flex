namespace Unic.Flex.Core.Context
{
    using System.Collections.Generic;
    using Sitecore.Data.Items;
    using Unic.Flex.Model.Forms;
    using Unic.Flex.Model.Steps;

    /// <summary>
    /// Service containing context based business logic.
    /// </summary>
    public interface IContextService
    {
        /// <summary>
        /// Loads the form based on a datasource.
        /// </summary>
        /// <param name="dataSourcebool">The data sourcebool.</param>
        /// <param name="useVersionCountDisabler">if set to <c>true</c> version count disabler is used to load the form.</param>
        /// <returns>
        /// The loaded form domain model object.
        /// </returns>
        IForm LoadForm(string dataSourcebool, bool useVersionCountDisabler = false);

        /// <summary>
        /// Populates the form values from the session into the form.
        /// </summary>
        /// <param name="form">The form domain model.</param>
        void PopulateFormValues(IForm form);

        /// <summary>
        /// Populates the form values from a dictionary.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="values">The values.</param>
        void PopulateFormValues(IForm form, IDictionary<string, object> values);

        /// <summary>
        /// Stores the form values into the session.
        /// </summary>
        /// <param name="form">The form.</param>
        void StoreFormValues(IForm form);

        /// <summary>
        /// Determines whether the given step can be actually accessed. This is only valid if all previous steps has been processed.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="step">The step.</param>
        /// <returns>Boolean value if the step may accessed by the user or not</returns>
        bool IsStepAccessible(IForm form, StepBase step);

        /// <summary>
        /// Gets the rendering datasource of a form.
        /// </summary>
        /// <param name="item">The item to search for a referenced form.</param>
        /// <param name="device">The device.</param>
        /// <returns>Datasource/form id if form is included on item</returns>
        string GetRenderingDatasource(Item item, DeviceItem device);
    }
}
