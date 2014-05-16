namespace Unic.Flex.Mapping
{
    using Glass.Mapper.Sc;
    using Sitecore.Diagnostics;
    using Unic.Flex.Model.Forms;

    public class FormRepository : IFormRepository
    {
        public Form LoadForm(string dataSource, ISitecoreContext sitecoreContext)
        {
            Assert.ArgumentCondition(Sitecore.Data.ID.IsID(dataSource), dataSource, "Datasource is not valid");
            return sitecoreContext.GetItem<Form>(dataSource, inferType: true);
        }
    }
}
