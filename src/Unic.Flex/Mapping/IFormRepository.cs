namespace Unic.Flex.Mapping
{
    using Glass.Mapper.Sc;
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
        /// <param name="sitecoreContext">The sitecore context.</param>
        /// <returns>The loaded form domain model</returns>
        Form LoadForm(string dataSource, ISitecoreContext sitecoreContext);
    }
}
