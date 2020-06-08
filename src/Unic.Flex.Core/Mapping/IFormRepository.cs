namespace Unic.Flex.Core.Mapping
{
    using System;
    using Unic.Flex.Model.Forms;

    /// <summary>
    /// Repository containg data access for the forms.
    /// </summary>
    public interface IFormRepository
    {
        /// <summary>
        /// Loads a form based on the data source from Sitecore.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <param name="useVersionCountDisabler">if set to <c>true</c> the version count disabler is used to load the form.</param>
        /// <returns>
        /// The loaded form domain model
        /// </returns>
        IForm LoadForm(string dataSource, bool useVersionCountDisabler = false);

        /// <summary>
        /// Loads an item from the data source.
        /// </summary>
        /// <typeparam name="T">Type of the item to load</typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Item if found
        /// </returns>
        T LoadItem<T>(Guid id) where T : class;

        /// <summary>
        /// Loads an item from the data source.
        /// </summary>
        /// <typeparam name="T">Type of the item to load</typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="useVersionCountDisabler">Disable Version Count</param>
        /// <returns>
        /// Item if found
        /// </returns>
        T LoadItem<T>(Guid id, bool useVersionCountDisabler) where T : class;
    }
}
