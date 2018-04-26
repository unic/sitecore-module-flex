namespace Unic.Flex.Implementation.Services
{
    public interface IDoubleOptinLinkService
    {
        bool ValidateConfirmationLink(string optInFormId, string optInRecordId, string email, string optInHashFromLink);

        string CreateConfirmationLink(string formId, string toEmail, string optInRecordId);
    }
}
