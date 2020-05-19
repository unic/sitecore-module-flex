namespace Unic.Flex.Implementation.Mailers
{
    using Configuration;
    using Mvc.Mailer;
    using Plugs.SavePlugs;
    using Model.Forms;

    public interface ISavePlugMailer
    {
        MvcMailMessage GetMessage(IForm form, SendEmail plug);

        MailMessageConfiguration GetMailMessageByConfiguration(IForm form, SendEmail plug);
    }
}
