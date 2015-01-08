namespace Unic.Flex.Core.Mailing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Unic.Flex.Model.DomainModel.Fields;

    /// <summary>
    /// Mail service with mail related stuff
    /// </summary>
    public class MailService : IMailService
    {
        /// <summary>
        /// Replaces the email addresses placeholders from the config file.
        /// </summary>
        /// <param name="addresses">The addresses string.</param>
        /// <returns>
        /// String with replaced email addresses
        /// </returns>
        public virtual string ReplaceEmailAddressesFromConfig(string addresses)
        {
            return Regex.Replace(
                addresses,
                @"({)(.*?)(})",
                match => this.GetEmailAddressFromSettings(match.Groups[2].Value));
        }

        /// <summary>
        /// Gets the email address from configuration.
        /// </summary>
        /// <param name="configKey">The configuration key.</param>
        /// <returns>
        /// The configuration key
        /// </returns>
        public virtual string GetEmailAddressFromSettings(string configKey)
        {
            return Sitecore.Configuration.Settings.GetSetting(string.Format("Flex.EmailAddresses.{0}", configKey));
        }

        /// <summary>
        /// Replaces the tokens in the input content with the content of the field.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="fields">The fields.</param>
        /// <returns>
        /// Content with replaced values
        /// </returns>
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
