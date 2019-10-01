namespace Unic.Flex.Model.Validators
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Wrapper interface for <c>System.ComponentModel.DataAnnotations.IValidatableObject</c> which adds method to add
    /// validators to the object.
    /// </summary>
    public interface IValidatableObject : System.ComponentModel.DataAnnotations.IValidatableObject
    {

        /// <summary>Determines whether the specified object is valid.</summary>
        /// <param name="validationContext">The validation context.</param>
        /// <param name="requestedValidationType">The type of the validation</param>
        /// <returns>A collection that holds failed-validation information.</returns>
        IEnumerable<ValidationResult> Validate(ValidationContext validationContext, ValidationType requestedValidationType);

        /// <summary>
        /// Adds a validator to the field.
        /// </summary>
        /// <param name="validator">The validator.</param>
        void AddValidator(IValidator validator);
    }
}
