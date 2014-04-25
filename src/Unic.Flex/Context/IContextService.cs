namespace Unic.Flex.Context
{
    using Glass.Mapper.Sc;
    using Unic.Flex.DomainModel.Forms;

    public interface IContextService
    {
        IForm LoadForm(string dataSource, ISitecoreContext sitecoreContext);
    }
}
