namespace Unic.Flex.Implementation.Mailers
{
    using System.Collections.Generic;
    using Model.Fields;
    using Model.Forms;

    public interface IMailerHelper
    {
        string GetEmailFromFields(IEnumerable<IField> fields, IForm form);

        string GetEmailFromField(IField field, IForm form);

        string GetEmailAddresses(string globalConfig, string plugConfig, bool useGlobalConfig);
    }
}
