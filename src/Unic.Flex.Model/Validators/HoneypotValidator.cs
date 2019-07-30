namespace Unic.Flex.Model.Validators
{

    /// <summary>
    /// Honeypot validator requires empty values to be valid
    /// </summary>
    public class HoneypotValidator : ValidatorBase
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
                return "Field must be empty";
            }
        }

        /// <summary>
        /// Determines whether the specified value is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the value entered is valid, <c>false</c> otherwise
        /// </returns>
        public override bool IsValid(object value)
        {
            return string.IsNullOrWhiteSpace(value as string);
        }
    }
}
