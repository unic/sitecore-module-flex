namespace Unic.Flex.Core.Mailing
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Unic.Flex.Model.Fields;

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
            return Sitecore.Configuration.Settings.GetSetting($"Flex.EmailAddresses.{configKey}");
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
                    var field = fields.FirstOrDefault(f => !string.IsNullOrWhiteSpace(f.Key) && f.Key.Equals(match.Groups[2].Value, StringComparison.InvariantCultureIgnoreCase));
                    return field != null ? field.TextValue : match.Value;
                });
        }

        /// <summary>
        /// Replaces {Salutation} token with the values from the mapping of salutation to corresponding gender values
        /// </summary>
        /// <param name="content">Content in which we do the replacement</param>
        /// <param name="genderFieldValue">Gender field value</param>
        /// <param name="genderSalutationMapping">Mapping of gender values and salutations</param>
        /// <returns>Content with replaced values</returns>
        public virtual string ReplaceSalutationToken(string content, string genderFieldValue, NameValueCollection genderSalutationMapping, IEnumerable<IField> fields)
        {
            if (genderSalutationMapping == null || string.IsNullOrEmpty(genderFieldValue)) return content;
            
            var selectedGenderMapping = genderSalutationMapping.Get(genderFieldValue);
            if (string.IsNullOrEmpty(selectedGenderMapping)) return content;
            
            var selectedGenderMappingWithReplacedTokens = this.ReplaceTokens(selectedGenderMapping, fields);

            return Regex.Replace(
                content,
                @"({)(salutation)(})",
                match =>
                {
                    if (!string.IsNullOrEmpty(selectedGenderMappingWithReplacedTokens))
                    {
                        return selectedGenderMappingWithReplacedTokens;
                    }

                    return match.Value;
                },
                RegexOptions.IgnoreCase);
        }
    }
}