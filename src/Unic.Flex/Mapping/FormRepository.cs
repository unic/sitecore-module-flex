namespace Unic.Flex.Mapping
{
    using Glass.Mapper.Sc;
    using Sitecore.Diagnostics;
    using Unic.Flex.Model.Forms;

    /// <summary>
    /// Repository containg data access for the forms.
    /// </summary>
    public class FormRepository : IFormRepository
    {
        /// <summary>
        /// Loads a form based on the data source from Sitecore.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <param name="sitecoreContext">The sitecore context.</param>
        /// <returns>
        /// The loaded form domain model
        /// </returns>
        public Form LoadForm(string dataSource, ISitecoreContext sitecoreContext)
        {
            Assert.ArgumentCondition(Sitecore.Data.ID.IsID(dataSource), dataSource, "Datasource is not valid");
            return sitecoreContext.GetItem<Form>(dataSource, inferType: true);
        }
    }
}
