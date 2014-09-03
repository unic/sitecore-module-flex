namespace Unic.Flex.Implementation.Mailers
{
    using Mvc.Mailer;

    public interface ISavePlugMailer
    {
        MvcMailMessage GetMessage();
    }
}
