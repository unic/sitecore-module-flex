namespace Unic.Flex.Model.DomainModel.Plugs.LoadPlugs
{
    using Unic.Flex.Model.DomainModel.Forms;

    public interface ILoadPlug
    {
        string ErrorMessage { get; set; }

        void Execute(Form form);
    }
}
