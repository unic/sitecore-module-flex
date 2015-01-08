namespace Unic.Flex.Core.Mapping
{
    using System;
    using Glass.Mapper.Sc;
    using Sitecore.Diagnostics;
    using Unic.Flex.Model.DomainModel.Forms;

    /// <summary>
    /// Repository containg data access for the forms.
    /// </summary>
    public class FormRepository : IFormRepository
    {
        /// <summary>
        /// The sitecore context
        /// </summary>
        private readonly ISitecoreContext sitecoreContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormRepository"/> class.
        /// </summary>
        /// <param name="sitecoreContext">The sitecore context.</param>
        public FormRepository(ISitecoreContext sitecoreContext)
        {
            this.sitecoreContext = sitecoreContext;
        }

        /// <summary>
        /// Loads a form based on the data source from Sitecore.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <returns>
        /// The loaded form domain model
        /// </returns>
        public virtual Form LoadForm(string dataSource)
        {
            Assert.ArgumentCondition(Sitecore.Data.ID.IsID(dataSource), dataSource, "Datasource is not valid");
            return this.sitecoreContext.GetItem<Form>(dataSource, inferType: true);
        }

        /// <summary>
        /// Loads an item from the data source.
        /// </summary>
        /// <typeparam name="T">Type of the item to load</typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Item if found
        /// </returns>
        public virtual T LoadItem<T>(Guid id) where T : class
        {
            return this.sitecoreContext.GetItem<T>(id, inferType: true);
        }
    }
}
