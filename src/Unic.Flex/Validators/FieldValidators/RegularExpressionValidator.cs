namespace Unic.Flex.Validators.FieldValidators
{
    using Sitecore.Data.Validators;
    using Sitecore.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Sitecore validator to check for a valid regular expression.
    /// </summary>
    public class RegularExpressionValidator : StandardValidator
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public override string Name
        {
            get
            {
                return "Regular Expression Validator";
            }
        }
        
        /// <summary>
        /// Evaluates the content of a field.
        /// </summary>
        /// <returns>ValidatorResult which describes if the content is valid or not</returns>
        protected override ValidatorResult Evaluate()
        {
            var field = this.GetField();
            var fieldValue = field.GetValue(true, true);

            if (string.IsNullOrWhiteSpace(fieldValue)) return ValidatorResult.Valid;

            try
            {
                Regex.Match(string.Empty, fieldValue);
                return ValidatorResult.Valid;
            }
            catch
            {
                this.Text = Translate.Text("The regular expression in field \"{0}\" is not valid", field.Name);
                return this.GetFailedResult(ValidatorResult.Error);   
            }
        }

        /// <summary>
        /// Gets the max validator result.
        /// </summary>
        /// <remarks>
        /// This is used when saving and the validator uses a thread. If the Max Validator Result
        /// is Error or below, the validator does not have to be evaluated before saving.
        /// If the Max Validator Result is CriticalError or FatalError, the validator must have
        /// been evaluated before saving.
        /// </remarks>
        /// <returns>
        /// The max validator result.
        /// </returns>
        protected override ValidatorResult GetMaxValidatorResult()
        {
            return this.GetFailedResult(ValidatorResult.Error);
        }
    }
}
