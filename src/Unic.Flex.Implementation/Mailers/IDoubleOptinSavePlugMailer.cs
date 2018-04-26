namespace Unic.Flex.Implementation.Mailers
{
    using Model.Forms;
    using Mvc.Mailer;
    using Plugs.SavePlugs;

    public interface IDoubleOptinSavePlugMailer
    {
        MvcMailMessage GetMessage(IForm form, DoubleOptinSavePlug plug, string doubleOptinLink);
    }
}
