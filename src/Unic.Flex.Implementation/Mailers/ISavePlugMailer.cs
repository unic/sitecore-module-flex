namespace Unic.Flex.Implementation.Mailers
{
    using Mvc.Mailer;
    using Unic.Flex.Model.DomainModel.Forms;

    public interface ISavePlugMailer
    {
        MvcMailMessage GetMessage(Form form);
    }
}
