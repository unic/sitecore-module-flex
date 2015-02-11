namespace Unic.Flex.Implementation.Validators
{
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Security;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.GlassExtensions.Attributes;
    using Unic.Flex.Model.Validation;

    /// <summary>
    /// Validates the password strength from a value
    /// </summary>
    [SitecoreType(TemplateId = "{007EB78D-C72D-4E31-B447-194E64349ABA}")]
    public class PasswordStrengthValidator : AjaxValidator
    {
        /// <summary>
        /// Gets the default validation message dictionary key.
        /// </summary>
        /// <value>
        /// The default validation message dictionary key.
        /// </value>
        public override string DefaultValidationMessageDictionaryKey
        {
            get
            {
                return "Password that does not meet the strength requirements defined by the policy";
            }
        }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message.
        /// </value>
        [SitecoreDictionaryFallbackField("Validation Message", "Password that does not meet the strength requirements defined by the policy")]
        public override string ValidationMessage { get; set; }

        /// <summary>
        /// Determines whether the specified value is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the value entered is valid, <c>false</c> otherwise
        /// </returns>
        public override bool IsValid(object value)
        {
            var stringValue = value as string;
            if (string.IsNullOrWhiteSpace(stringValue)) return true;

            // check length of password
            if (stringValue.Length < Membership.MinRequiredPasswordLength) return false;

            // check minimal count of non alhanumeric chars
            if (stringValue.Where((t, i) => !char.IsLetterOrDigit(stringValue, i)).Count() < Membership.MinRequiredNonAlphanumericCharacters) return false;

            // check regular expression for password
            if (Membership.PasswordStrengthRegularExpression.Length > 0 && !Regex.IsMatch(stringValue, Membership.PasswordStrengthRegularExpression)) return false;

            // everything ok :)
            return true;
        }
    }
}
