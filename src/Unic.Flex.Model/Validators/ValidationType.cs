namespace Unic.Flex.Model.Validators
{
    /// <summary>
    /// Types for Validations
    /// </summary>
    public enum ValidationType
    {
        /// <summary>
        /// Validator gets executed after Field Binding
        /// default, validation in the field context
        /// </summary>
        FieldValidation,
        /// <summary>
        /// Validator gets executed after Form Binding
        /// Needed if there are dependencies to other fields
        /// </summary>
        FormValidation
    }
}