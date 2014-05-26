namespace Unic.Flex.Model.Validation
{
    public interface IValidatableObject : System.ComponentModel.DataAnnotations.IValidatableObject
    {
        void AddValidator(IValidator validator);
    }
}
