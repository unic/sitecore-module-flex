namespace Unic.Flex.Implementation.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Context;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.Model.DomainModel.Fields;
    using Unic.Flex.Model.GlassExtensions.Attributes;
    using Unic.Flex.Model.Validation;

    /// <summary>
    /// Validator which checks if two fields are equal
    /// </summary>
    [SitecoreType(TemplateId = "{90C8A94E-EDEE-41B6-ACF3-4C264B9FDBE1}")]
    public class CompareValidator : IValidator
    {
        /// <summary>
        /// Gets the default validation message dictionary key.
        /// </summary>
        /// <value>
        /// The default validation message dictionary key.
        /// </value>
        public virtual string DefaultValidationMessageDictionaryKey
        {
            get
            {
                return "Fields are not equal";
            }
        }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message.
        /// </value>
        [SitecoreDictionaryFallbackField("Validation Message", "Fields are not equal")]
        public virtual string ValidationMessage { get; set; }

        /// <summary>
        /// Gets or sets the other field model.
        /// </summary>
        /// <value>
        /// The other field model.
        /// </value>
        [SitecoreField("Other Field", Setting = SitecoreFieldSettings.InferType)]
        public virtual FieldBase OtherFieldModel { get; set; }

        /// <summary>
        /// Gets the other field display name.
        /// </summary>
        /// <value>
        /// The other field display name.
        /// </value>
        public virtual string OtherField
        {
            get
            {
                return this.OtherFieldModel != null ? this.OtherFieldModel.Label : string.Empty;
            }
        }

        /// <summary>
        /// Determines whether the specified value is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the value entered is valid, <c>false</c> otherwise
        /// </returns>
        public bool IsValid(object value)
        {
            // other field not configured or not found
            if (this.OtherField == null) return false;
            
            // get the other field
            var context = Container.Resolve<IFlexContext>();
            var field = context.Form.GetAllFields(context.Form.GetActiveStep().StepNumber).FirstOrDefault(f => f.ItemId == this.OtherFieldModel.ItemId);
            if (field == null) return false;

            // todo: this does not work as the field.Value is always null because it's not set in this context here -> we are in modelbinding and the "value" parameters comes from the view model and not from the domain model
            
            // compare field values
            return (value == null && field.Value == null) || (value != null && value.Equals(field.Value));
        }

        /// <summary>
        /// Gets the additional html attributes which should be rendered.
        /// </summary>
        /// <returns>
        /// Key-Value based dictionary with additional html attributes
        /// </returns>
        public virtual IDictionary<string, object> GetAttributes()
        {
            var attributes = new Dictionary<string, object>();
            //attributes.Add("data-val-equalto", this.ValidationMessage);
            //attributes.Add("data-val-equalto-other", "*.Name1"); // todo: set to the correct value
            return attributes;
        }
    }
}
