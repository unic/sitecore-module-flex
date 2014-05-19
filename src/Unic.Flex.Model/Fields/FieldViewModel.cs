namespace Unic.Flex.Model.Fields
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Unic.Flex.Model;
    using Unic.Flex.Model.Presentation;
    using Unic.Flex.Model.Validators;
    using Unic.Flex.Presentation;

    public class FieldViewModel : IPresentationComponent, IViewModel, IValidatableObject
    {
        private readonly IList<IValidator> validators;
        
        public FieldViewModel(ItemBase domainModel)
        {
            this.DomainModel = domainModel;
            this.Attributes = new Dictionary<string, object>();
            this.validators = new List<IValidator>();
        }
        
        public string Key { get; set; }

        public string Label { get; set; }

        public string Value { get; set; } // todo: string is wrong here, this must be object

        public IDictionary<string, object> Attributes { get; private set; }

        public string ViewName { get; set; }

        public ItemBase DomainModel { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var validator in this.validators)
            {
                if (!validator.IsValid(this.Value))
                {
                    yield return new ValidationResult(validator.ValidationMessage, new[] { "Value" });
                }
            }
        }

        public void AddValidator(IValidator validator)
        {
            this.validators.Add(validator);
            foreach (var attribute in validator.GetAttributes())
            {
                this.Attributes.Add(attribute.Key, attribute.Value);
            }

            if (!this.Attributes.ContainsKey("data-val"))
            {
                this.Attributes.Add("data-val", "true");
            }
        }
    }
}