using ISitecoreContext = Glass.Mapper.Sc.ISitecoreContext;

namespace Unic.Flex.Service.Glass
{
    using Sitecore.Diagnostics;
    using Unic.Flex.DomainModel.Forms;

    public class FormRepository : IFormRepository
    {
        public IForm LoadForm(string dataSource, ISitecoreContext sitecoreContext)
        {
            Assert.ArgumentCondition(Sitecore.Data.ID.IsID(dataSource), dataSource, "Datasource is not valid");
            return sitecoreContext.GetItem<IForm>(dataSource, inferType: true);
        }
    }
}
