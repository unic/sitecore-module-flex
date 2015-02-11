namespace Unic.Flex.Model.Validation
{
    /// <summary>
    /// Wrapper interface for <c>System.ComponentModel.DataAnnotations.IValidatableObject</c> which adds method to add
    /// validators to the object.
    /// </summary>
    public interface IValidatableObject : System.ComponentModel.DataAnnotations.IValidatableObject
    {
        /// <summary>
        /// Adds a validator to the field.
        /// </summary>
        /// <param name="validator">The validator.</param>
        void AddValidator(IValidator validator);
    }
}
