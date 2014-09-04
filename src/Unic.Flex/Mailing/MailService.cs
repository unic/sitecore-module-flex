using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Mailing
{
    using System.Text.RegularExpressions;
    using Unic.Flex.Model.DomainModel.Fields;
    using Unic.Flex.Model.DomainModel.Forms;

    public class MailService : IMailService
    {
        public virtual string ReplaceEmailAddressesFromConfig(string addresses)
        {
            return Regex.Replace(
                addresses,
                @"({)(.*?)(})",
                match => Sitecore.Configuration.Settings.GetSetting(string.Format("Flex.EmailAddresses.{0}", match.Groups[2].Value)));
        }

        public virtual string ReplaceTokens(string content, IEnumerable<IField> fields)
        {
            return Regex.Replace(
                content,
                @"({)(.*?)(})",
                match =>
                    {
                        var field = fields.FirstOrDefault(f => f.Key.Equals(match.Groups[2].Value, StringComparison.InvariantCultureIgnoreCase));
                        return field != null ? field.TextValue : match.Value;
                    });
        }
    }
}
