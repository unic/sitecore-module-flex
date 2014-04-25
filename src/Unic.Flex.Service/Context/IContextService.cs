using ISitecoreContext = Glass.Mapper.Sc.ISitecoreContext;

namespace Unic.Flex.Service.Context
{
    using Unic.Flex.DomainModel.Forms;

    public interface IContextService
    {
        IForm LoadForm(string dataSource, ISitecoreContext sitecoreContext);
    }
}
