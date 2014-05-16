namespace Unic.Flex.Mapping
{
    using Glass.Mapper.Sc;
    using Unic.Flex.Model.Forms;

    public interface IFormRepository
    {
        Form LoadForm(string dataSource, ISitecoreContext sitecoreContext);
    }
}
