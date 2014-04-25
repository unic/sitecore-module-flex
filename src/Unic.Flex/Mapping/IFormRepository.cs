namespace Unic.Flex.Mapping
{
    using Glass.Mapper.Sc;
    using Unic.Flex.DomainModel.Forms;

    public interface IFormRepository
    {
        IForm LoadForm(string dataSource, ISitecoreContext sitecoreContext);
    }
}
