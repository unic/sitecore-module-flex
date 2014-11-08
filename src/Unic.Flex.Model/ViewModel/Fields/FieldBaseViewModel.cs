﻿namespace Unic.Flex.Model.ViewModel.Fields
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using Castle.DynamicProxy;
    using Unic.Flex.Model.Validation;
    using Unic.Flex.Model.ViewModel.Components;
    using IValidatableObject = Unic.Flex.Model.Validation.IValidatableObject;

    /// <summary>
    /// This view model covers a field in the form
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public abstract class FieldBaseViewModel<TValue> : IValidatableObject, IFieldViewModel<TValue>
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
            this.ContainerAttributes = new Dictionary<string, object>();
            this.validators = new List<IValidator>();
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        object IFieldViewModel.Value
        {
            get
            {
                return this.Value;
            }

            set
            {
                this.Value = value != null ? (TValue)value : default(TValue);
            }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public virtual Type Type
        {
            get
            {
                return typeof(TValue);
            }
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public virtual string Id { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public virtual string Label { get; set; }

        /// <summary>
        /// Gets or sets the label addition.
        /// </summary>
        /// <value>
        /// The label addition.
        /// </value>
        public virtual string LabelAddition { get; set; }

        /// <summary>
        /// Gets or sets the dependent field.
        /// </summary>
        /// <value>
        /// The dependent field.
        /// </value>
        public virtual string DependentFrom { get; set; }

        /// <summary>
        /// Gets or sets the dependent value.
        /// </summary>
        /// <value>
        /// The dependent value.
        /// </value>
        public virtual string DependentValue { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public virtual TValue Value { get; set; }

        /// <summary>
        /// Gets or sets the text value.
        /// </summary>
        /// <value>
        /// The text value.
        /// </value>
        public virtual string TextValue { get; set; }

        /// <summary>
        /// Gets the additional html attributes on the field.
        /// </summary>
        /// <value>
        /// The additional html attributes on the field.
        /// </value>
        public virtual IDictionary<string, object> Attributes { get; private set; }

        /// <summary>
        /// Gets the container attributes.
        /// </summary>
        /// <value>
        /// The container attributes.
        /// </value>
        public virtual IDictionary<string, object> ContainerAttributes { get; private set; } 

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public abstract string ViewName { get; }

        /// <summary>
        /// Gets or sets the CSS class.
        /// </summary>
        /// <value>
        /// The CSS class.
        /// </value>
        public virtual string CssClass { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this field is disabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this field is disabled; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsDisabled { get; set; }

        /// <summary>
        /// Gets or sets the tooltip.
        /// </summary>
        /// <value>
        /// The tooltip.
        /// </value>
        public virtual TooltipViewModel Tooltip { get; set; }

        /// <summary>
        /// Binds the needed attributes and properties after converting from domain model to the view model
        /// </summary>
        public virtual void BindProperties()
        {
            // handle disabled input fields
            if (this.IsDisabled)
            {
                this.Attributes.Add("disabled", "disabled");
                this.Attributes.Add("aria-disabled", true);
                this.AddCssClass("flex_disabled");
            }

            // handle field dependency
            this.ContainerAttributes.Add("data-key", this.Id);
            if (!string.IsNullOrWhiteSpace(this.DependentFrom))
            {
                this.ContainerAttributes.Add("data-flexform-dependent", "{" + HttpUtility.HtmlEncode(string.Format("\"from\": \"{0}\", \"value\": \"{1}\"", this.DependentFrom, this.DependentValue)) + "}");   
            }
        }

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
            // do nothing if validators of this type has already been added
            var validatorType = ProxyUtil.GetUnproxiedType(validator);
            if (this.validators.Any(v => ProxyUtil.GetUnproxiedType(v) == validatorType)) return;

            // replace the placeholders of the validator with values
            validator.ValidationMessage = this.ReplaceValidatorMessagePlaceholders(validator);

            // add the validator to the list
            this.validators.Add(validator);

            // add the specific attributes for this validator (i.e. "data-val-requried")
            // these are used for client side validation
            foreach (var attribute in validator.GetAttributes())
            {
                this.Attributes.Add(attribute.Key, attribute.Value);
            }

            // add the "data-val" attribute to specify that this field needs to be validated
            if (this.Attributes.Any() && !this.Attributes.ContainsKey("data-val"))
            {
                this.Attributes.Add("data-val", "true");
            }
        }

        /// <summary>
        /// Adds a CSS class.
        /// </summary>
        /// <param name="cssClass">The CSS class.</param>
        public virtual void AddCssClass(string cssClass)
        {
            if (string.IsNullOrWhiteSpace(cssClass)) return;

            this.CssClass = string.Join(" ", this.CssClass, cssClass);
        }

        /// <summary>
        /// Replaces the validator message placeholders with the value of the validators.
        /// </summary>
        /// <param name="validator">The validator.</param>
        /// <returns>The replaced validator message</returns>
        private string ReplaceValidatorMessagePlaceholders(IValidator validator)
        {
            // get the type of the validator
            var type = validator.GetType();
            
            // replace all placeholders
            var message = Regex.Replace(
                validator.ValidationMessage,
                @"({)(.*?)(})",
                match => type.GetProperty(match.Groups[2].Value) != null
                    ? type.GetProperty(match.Groups[2].Value).GetValue(validator).ToString()
                    : match.Value);

            // replace the field name
            message = message.Replace("{Field}", this.Label);

            return message;
        }
    }
}