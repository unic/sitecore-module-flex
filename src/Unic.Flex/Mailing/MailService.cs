using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Mailing
{
    using System.Text.RegularExpressions;

    public class MailService : IMailService
    {
        public virtual string ReplaceEmailAddressesFromConfig(string addresses)
        {
            return Regex.Replace(
                addresses,
                @"({)(.*?)(})",
                match => Sitecore.Configuration.Settings.GetSetting(string.Format("Flex.EmailAddresses.{0}", match.Groups[2].Value)));
        }
    }
}
