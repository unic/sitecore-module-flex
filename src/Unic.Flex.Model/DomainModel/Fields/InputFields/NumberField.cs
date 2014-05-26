﻿namespace Unic.Flex.Model.DomainModel.Fields.InputFields
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.DomainModel.Validators;

    /// <summary>
    /// Number field
    /// </summary>
    [SitecoreType(TemplateId = "{C9C6AC45-7763-4851-A684-A075D2176FF7}")]
    public class NumberField : InputField<int?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberField"/> class.
        /// </summary>
        public NumberField()
        {
            this.DefaultValidators.Add(new NumberValidator { ValidationMessage = NumberValidator.ValidationMessageDictionaryKey });
        }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public override string ViewName
        {
            get
            {
                return "Fields/InputFields/Number";
            }
        }
    }
}
