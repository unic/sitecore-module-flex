namespace Unic.Flex.Validators.FieldValidators
{
    using Sitecore.Data.Validators;
    using Unic.Flex.Globalization;
    using Unic.Flex.Utilities;

    /// <summary>
    /// Validates if the references item is a form field.
    /// </summary>
    public class FormFieldValidator : StandardValidator
    {
        /// <summary>
        /// The field template identifier
        /// </summary>
        private const string FieldTemplateId = "{0AB071EF-1DBE-42F5-8A22-002B9110E90F}";
        
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
                return "Form Field Validator";
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

            // no field referenced is valid
            if (string.IsNullOrWhiteSpace(fieldValue)) return ValidatorResult.Valid;

            // check if item has valid base template
            var item = this.GetItem().Database.GetItem(fieldValue);
            if (item != null && item.HasBaseTemplate(FieldTemplateId)) return ValidatorResult.Valid;
            
            // referenced item is not valid
            this.Text = TranslationHelper.FlexText(string.Format("The item referenced in field \"{0}\" is not a valid form field", field.Name));
            return this.GetFailedResult(ValidatorResult.Error);
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
