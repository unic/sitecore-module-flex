namespace Unic.Flex.Model.ViewModel.Fields
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Unic.Flex.Model.DomainModel;
    using Unic.Flex.Model.Presentation;
    using Unic.Flex.Model.Validation;

    /// <summary>
    /// This view model covers a field in the form
    /// </summary>
    public abstract class FieldBaseViewModel<TValue> : IPresentationComponent, IValidatableObject
    {
        /// <summary>
        /// The validators
        /// </summary>
        private readonly IList<IValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldBaseViewModel{TValue}"/> class.
        /// </summary>
        protected FieldBaseViewModel()
        {
            this.Attributes = new Dictionary<string, object>();
            this.validators = new List<IValidator>();
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public virtual string Key { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public virtual string Label { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public virtual TValue Value { get; set; }

        /// <summary>
        /// Gets the additional html attributes on the field.
        /// </summary>
        /// <value>
        /// The additional html attributes on the field.
        /// </value>
        public virtual IDictionary<string, object> Attributes { get; private set; }

        /// <summary>
        /// Gets or sets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public virtual string ViewName { get; set; }

        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>
        /// A collection that holds failed-validation information.
        /// </returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var validator in this.validators)
            {
                if (!validator.IsValid(this.Value))
                {
                    yield return new ValidationResult(validator.ValidationMessage, new[] { "Value" });
                }
            }
        }

        /// <summary>
        /// Adds a validator to the current field.
        /// </summary>
        /// <param name="validator">The validator.</param>
        public virtual void AddValidator(IValidator validator)
        {
            // add the validator to the list
            this.validators.Add(validator);

            // add the specific attributes for this validator (i.e. "data-val-requried")
            // these are used for client side validation
            foreach (var attribute in validator.GetAttributes())
            {
                this.Attributes.Add(attribute.Key, attribute.Value);
            }

            // add the "data-val" attribute to specify that this field needs to be validated
            if (!this.Attributes.ContainsKey("data-val"))
            {
                this.Attributes.Add("data-val", "true");
            }
        }
    }
}