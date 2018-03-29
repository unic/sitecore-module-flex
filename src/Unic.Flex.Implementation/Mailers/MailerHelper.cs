namespace Unic.Flex.Implementation.Mailers
{
    using System.Collections.Generic;
    using Core.Mailing;
    using Model.Fields;
    using Model.Forms;
    using Validators;

    public class MailerHelper : IMailerHelper
    {
        private readonly IMailService mailService;

        public MailerHelper(IMailService mailService)
        {
            this.mailService = mailService;
        }

        public string GetEmailFromFields(IEnumerable<IField> fields, IForm form)
        {
            foreach (var field in fields)
            {
                var email = GetEmailFromField(field, form);
                if (!string.IsNullOrWhiteSpace(email)) return email;
            }

            return string.Empty;
        }

        public string GetEmailFromField(IField field, IForm form)
        {
            var fieldValue = form.GetFieldValue(field);
            return (!string.IsNullOrWhiteSpace(fieldValue) && (new EmailValidator()).IsValid(fieldValue)) ? fieldValue : string.Empty;
        }
       
        public string GetEmailAddresses(string globalConfig, string plugConfig, bool useGlobalConfig)
        {
            return mailService.ReplaceEmailAddressesFromConfig(useGlobalConfig || string.IsNullOrWhiteSpace(plugConfig) ? globalConfig : plugConfig);
        }
    }
}
