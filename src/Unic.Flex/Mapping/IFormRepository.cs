namespace Unic.Flex.Mapping
{
    using System;
    using Unic.Flex.Model.DomainModel.Forms;

    /// <summary>
    /// Repository containg data access for the forms.
    /// </summary>
    public interface IFormRepository
    {
        /// <summary>
        /// Loads a form based on the data source from Sitecore.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <returns>
        /// The loaded form domain model
        /// </returns>
        Form LoadForm(string dataSource);

        /// <summary>
        /// Loads an item from the data source.
        /// </summary>
        /// <typeparam name="T">Type of the item to load</typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Item if found
        /// </returns>
        T LoadItem<T>(Guid id) where T : class;
    }
}
