using ISitecoreContext = Glass.Mapper.Sc.ISitecoreContext;

namespace Unic.Flex.Service.Glass
{
    using Unic.Flex.DomainModel.Forms;

    public interface IFormRepository
    {
        IForm LoadForm(string dataSource, ISitecoreContext sitecoreContext);
    }
}
