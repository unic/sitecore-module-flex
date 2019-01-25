namespace Unic.Flex.Core.Mailing
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Unic.Flex.Model.Fields;

    /// <summary>
    /// Mail service with mail related stuff
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Replaces the email addresses placeholders from the config file.
        /// </summary>
        /// <param name="addresses">The addresses string.</param>
        /// <returns>String with replaced email addresses</returns>
        string ReplaceEmailAddressesFromConfig(string addresses);

        /// <summary>
        /// Gets the email address from configuration.
        /// </summary>
        /// <param name="configKey">The configuration key.</param>
        /// <returns>The configuration key</returns>
        string GetEmailAddressFromSettings(string configKey);

        /// <summary>
        /// Replaces the tokens in the input content with the content of the field.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="fields">The fields.</param>
        /// <returns>Content with replaced values</returns>
        string ReplaceTokens(string content, IEnumerable<IField> fields);

        /// <summary>
        /// Replaces {Salutation} token with the values from the mapping of salutation to corresponding gender values
        /// </summary>
        /// <param name="content">Content in which we do the replacement</param>
        /// <param name="genderField">Field with selected gender value</param>
        /// <param name="genderSalutationMapping">Mapping of gender values and salutations</param>
        /// <returns>Content with replaced values</returns>
        string ReplaceSalutationToken(string content, IField genderField, NameValueCollection genderSalutationMapping);
    }
}