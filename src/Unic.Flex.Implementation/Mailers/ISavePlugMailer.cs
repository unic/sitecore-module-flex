namespace Unic.Flex.Implementation.Mailers
{
    using Mvc.Mailer;
    using Unic.Flex.Implementation.Plugs.SavePlugs;
    using Unic.Flex.Model.DomainModel.Forms;

    public interface ISavePlugMailer
    {
        MvcMailMessage GetMessage(Form form, SendEmail plug);
    }
}
