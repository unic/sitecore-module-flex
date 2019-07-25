namespace Unic.Flex.Model.Fields
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using Castle.DynamicProxy;
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Glass.Mapper.Sc.Fields;
    using Unic.Flex.Model.Components;
    using Unic.Flex.Model.GlassExtensions.Attributes;
    using Unic.Flex.Model.Specifications;
    using Unic.Flex.Model.Validators;

    /// <summary>
    /// Generic base class for all available fields
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public abstract class FieldBase<TValue> : FieldBase, IField<TValue>
    {
        /// <summary>
        /// The validators
        /// </summary>
        private readonly IList<IValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldBase{TValue}"/> class.
        /// </summary>
        protected FieldBase()
        {
            this.validators = new List<IValidator>();
        }
        
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [SitecoreIgnore]
        object IField.Value
        {
            get
            {
                return this.Value;
            }

            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        [SitecoreIgnore]
        object IField.DefaultValue
        {
            get
            {
                return this.DefaultValue;
            }

            set
            {
                this.DefaultValue = value != null ? (TValue)value : default(TValue);
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [SitecoreIgnore]
        public virtual TValue Value { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        [SitecoreIgnore]
        public virtual TValue DefaultValue { get; set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [SitecoreIgnore]
        public virtual Type Type
        {
            get
            {
                return typeof(TValue);
            }
        }

        /// <summary>
        /// Gets the text value.
        /// </summary>
        /// <value>
        /// The text value.
        /// </value>
        [SitecoreIgnore]
        public virtual string TextValue
        {
            get
            {
                var value = !Equals(this.Value, null) ? this.Value.ToString() : string.Empty;
                return !string.IsNullOrWhiteSpace(value) ? value : Definitions.Constants.EmptyFlexFieldDefaultValue;
            }
        }

        /// <summary>
        /// Get's the Name for the Field in the Dom Model
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>
        /// A collection that holds failed-validation information.
        /// </returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return this.Validate(validationContext, ValidationType.FieldValidation);
        }

        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <param name="type">The validation type</param>
        /// <returns>
        /// A collection that holds failed-validation information.
        /// </returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext, ValidationType type)
        {
            foreach (var validator in this.validators)
            {
                if (validator.Type == type && !validator.IsValid(this.Value))
                {
                    if (!this.Attributes.ContainsKey("aria-invalid"))
                    {
                        this.Attributes.Add("aria-invalid", "true");
                    }

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
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public virtual void SetValue(object value)
        {
            this.Value = value != null ? (TValue)value : default(TValue);
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

    /// <summary>
    /// Abstract base class for all fields
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public abstract class FieldBase : ItemBase, IReusableComponent<IField>
    {
        /// <summary>
        /// Private field for storing the is hidden property.
        /// </summary>
        private bool? isHidden;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldBase"/> class.
        /// </summary>
        protected FieldBase()
        {
            this.DefaultValidators = new List<IValidator>();
            this.Attributes = new Dictionary<string, object>();
            this.ContainerAttributes = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [SitecoreField("Key")]
        public virtual string Key { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        [SitecoreField("Label")]
        public virtual string Label { get; set; }

        /// <summary>
        /// Gets or sets the label link.
        /// </summary>
        /// <value>
        /// The label link.
        /// </value>
        [SitecoreField("Label Link", UrlOptions = SitecoreInfoUrlOptions.AlwaysIncludeServerUrl)]
        public virtual Link LabelLink { get; set; }

        /// <summary>
        /// Gets or sets the CSS class.
        /// </summary>
        /// <value>
        /// The CSS class.
        /// </value>
        [SitecoreSharedField("Css Class")]
        public virtual Specification CustomCssClass { get; set; }

        /// <summary>
        /// Gets or sets the additional CSS classes.
        /// </summary>
        /// <value>
        /// The additional CSS classes.
        /// </value>
        [SitecoreField("Additional Css Class")]
        public virtual string AdditionalCssClass { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this field is required.
        /// </summary>
        /// <value>
        /// <c>true</c> if this field is required; otherwise, <c>false</c>.
        /// </value>
        [SitecoreField("Is Required")]
        public virtual bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message.
        /// </value>
        [SitecoreDictionaryFallbackField("Validation Message", "Field is required")]
        public virtual string ValidationMessage { get; set; }

        /// <summary>
        /// Gets or sets the validators.
        /// </summary>
        /// <value>
        /// The validators.
        /// </value>
        [SitecoreChildren(IsLazy = true, InferType = true)]
        public virtual IEnumerable<IValidator> Validators { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show this field in the summary or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this field should be shown in the summary; otherwise, <c>false</c>.
        /// </value>
        [SitecoreField("Show in Summary")]
        public virtual bool ShowInSummary { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this field is disabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this field is disabled; otherwise, <c>false</c>.
        /// </value>
        [SitecoreField("Is Disabled")]
        public virtual bool IsDisabled { get; set; }

        /// <summary>
        /// Gets or sets the tooltip title.
        /// </summary>
        /// <value>
        /// The tooltip title.
        /// </value>
        [SitecoreDictionaryFallbackField("Tooltip Title", "Tooltip Title")]
        public virtual string TooltipTitle { get; set; }

        /// <summary>
        /// Gets or sets the tooltip text.
        /// </summary>
        /// <value>
        /// The tooltip text.
        /// </value>
        [SitecoreField("Tooltip Text")]
        public virtual string TooltipText { get; set; }

        /// <summary>
        /// Gets or sets the dependent field.
        /// </summary>
        /// <value>
        /// The dependent field.
        /// </value>
        [SitecoreReusableField("Dependent Field", Setting = SitecoreFieldSettings.InferType)]
        public virtual IField DependentField { get; set; }

        /// <summary>
        /// Gets or sets the dependent value.
        /// </summary>
        /// <value>
        /// The dependent value.
        /// </value>
        [SitecoreField("Dependent Value")]
        public virtual string DependentValue { get; set; }

        /// <summary>
        /// Gets or sets the reusable component.
        /// </summary>
        /// <value>
        /// The reusable component.
        /// </value>
        [SitecoreField("Field", Setting = SitecoreFieldSettings.InferType)]
        public virtual IField ReusableComponent { get; set; }

        /// <summary>
        /// Gets or sets the tooltip.
        /// </summary>
        /// <value>
        /// The tooltip.
        /// </value>
        [SitecoreIgnore]
        public virtual Tooltip Tooltip { get; set; }

        /// <summary>
        /// Gets the default validators.
        /// </summary>
        /// <value>
        /// The default validators.
        /// </value>
        [SitecoreIgnore]
        public virtual IList<IValidator> DefaultValidators { get; private set; }

        /// <summary>
        /// Gets the text label.
        /// </summary>
        /// <value>
        /// The text label.
        /// </value>
        [SitecoreIgnore]
        public virtual string TextLabel
        {
            get
            {
                if (this.LabelLink != null && !string.IsNullOrWhiteSpace(this.LabelLink.Text))
                {
                    return this.Label.Replace("#link#", this.LabelLink.Text);
                }

                return this.Label;
            }
        }

        /// <summary>
        /// Gets or sets the label addition.
        /// </summary>
        /// <value>
        /// The label addition.
        /// </value>
        [SitecoreIgnore]
        public virtual string LabelAddition { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is hidden.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is hidden; otherwise, <c>false</c>.
        /// </value>
        [SitecoreIgnore]
        public virtual bool IsHidden
        {
            get
            {
                // lazy loading
                if (this.isHidden.HasValue) return this.isHidden.Value;

                // no dependent field -> always visible
                if (this.DependentField == null)
                {
                    this.isHidden = false;
                    return this.isHidden.Value;
                }

                // Check upwards the cascade of dependent fields if one of them is hidden. This is a recursion!
                if (this.DependentField.IsHidden)
                {
                    this.isHidden = true;
                    return this.isHidden.Value;
                }

                // get the value of the dependent field
                var dependentValue = this.DependentField.Value != null ? this.DependentField.Value.ToString() : string.Empty;
                var listValue = this.DependentField.Value as IEnumerable<string>;
                var orDependentValues = DependentValue.Split('|');

                // check if dependent values use or conjunction
                if (orDependentValues.Length > 1)
                {
                    this.isHidden = listValue != null
                        ? !orDependentValues.Intersect(listValue).Any()
                        : !orDependentValues.Contains(dependentValue);
                }
                else
                {
                    this.isHidden = listValue != null
                       // check if selected values contain all values from dependent field definition
                       ? this.DependentValue.Split(',').Except(listValue).Any()
                       : !dependentValue.Equals(this.DependentValue, StringComparison.InvariantCultureIgnoreCase);
                }

                return this.isHidden.Value;
            }
        }

        /// <summary>
        /// Gets or sets the reusable component.
        /// </summary>
        /// <value>
        /// The reusable component.
        /// </value>
        [SitecoreIgnore]
        object IReusableComponent.ReusableComponent
        {
            get
            {
                return this.ReusableComponent;
            }

            set
            {
                this.ReusableComponent = value as IField;
            }
        }

        /// <summary>
        /// Gets or sets the CSS class.
        /// </summary>
        /// <value>
        /// The CSS class.
        /// </value>
        [SitecoreIgnore]
        public virtual string CssClass { get; set; }

        /// <summary>
        /// Gets the additional html attributes on the field.
        /// </summary>
        /// <value>
        /// The additional html attributes on the field.
        /// </value>
        [SitecoreIgnore]
        public virtual IDictionary<string, object> Attributes { get; private set; }

        /// <summary>
        /// Gets the container attributes.
        /// </summary>
        /// <value>
        /// The container attributes.
        /// </value>
        [SitecoreIgnore]
        public virtual IDictionary<string, object> ContainerAttributes { get; private set; }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        [SitecoreIgnore]
        public abstract string ViewName { get; }

        /// <summary>
        /// Binds the properties.
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
            if (this.DependentField != null && !this.ContainerAttributes.ContainsKey("data-flexform-dependent"))
            {
                this.ContainerAttributes.Add("data-flexform-dependent", "{" + HttpUtility.HtmlEncode(string.Format("\"from\": \"{0}\", \"value\": \"{1}\"", this.DependentField.Id, this.DependentValue)) + "}");
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
    }
}
