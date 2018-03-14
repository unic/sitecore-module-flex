namespace Unic.Flex.Core.MarketingAutomation
{
    using System;

    public interface IMarketingAutomationContactService
    {
        object GetContactValue(Guid contactFieldDefinitionId);

        void SetContactValue(Guid contactFieldDefinitionId, object value);

        void SetContactValue(string simpleFieldName, string value);

        object GetContactValue(string simpleFieldName);

        string GetEmailAddress(string name);

        void SetEmailAddress(string name, string email);

        void IdentifyContactEmail(string name);
    }
}