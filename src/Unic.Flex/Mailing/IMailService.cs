namespace Unic.Flex.Mailing
{
    using System.Collections.Generic;
    using Unic.Flex.Model.DomainModel.Fields;

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
        /// Replaces the tokens in the input content with the content of the field.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="fields">The fields.</param>
        /// <returns>Content with replaced values</returns>
        string ReplaceTokens(string content, IEnumerable<IField> fields);
    }
}
