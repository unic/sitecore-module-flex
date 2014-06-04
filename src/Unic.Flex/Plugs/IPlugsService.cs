namespace Unic.Flex.Plugs
{
    using Unic.Flex.Model.DomainModel.Forms;

    public interface IPlugsService
    {
        void ExecuteLoadPlugs(Form form);

        void ExecuteSavePlugs(Form form);
    }
}
