namespace Unic.Flex.Mapping
{
    using System;
    using Glass.Mapper.Sc;
    using Sitecore.Diagnostics;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.Validation;

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
        public Form LoadForm(string dataSource)
        {
            Assert.ArgumentCondition(Sitecore.Data.ID.IsID(dataSource), dataSource, "Datasource is not valid");
            return this.sitecoreContext.GetItem<Form>(dataSource, inferType: true);
        }

        /// <summary>
        /// Loads a validator from the data source.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Validator if found
        /// </returns>
        public IValidator LoadValidator(Guid id)
        {
            return this.sitecoreContext.GetItem<IValidator>(id, inferType: true);
        }
    }
}
