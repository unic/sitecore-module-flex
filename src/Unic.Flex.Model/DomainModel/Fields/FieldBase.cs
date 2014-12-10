namespace Unic.Flex.Model.DomainModel.Fields
{
    using System;
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Unic.Flex.Model.DomainModel.Components;
    using Unic.Flex.Model.DomainModel.Global;
    using Unic.Flex.Model.GlassExtensions.Attributes;
    using Unic.Flex.Model.Validation;

    /// <summary>
    /// Generic base class for all available fields
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public abstract class FieldBase<TValue> : FieldBase, IField<TValue>
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
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
        public virtual TValue Value { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public virtual TValue DefaultValue { get; set; }

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
        /// Gets the text value.
        /// </summary>
        /// <value>
        /// The text value.
        /// </value>
        public virtual string TextValue
        {
            get
            {
                var value = !Equals(this.Value, null) ? this.Value.ToString() : string.Empty;
                return !string.IsNullOrWhiteSpace(value) ? value : "-";
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        protected virtual void SetValue(object value)
        {
            this.Value = value != null ? (TValue)value : default(TValue);
        }
    }

    /// <summary>
    /// Abstract base class for all fields
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public abstract class FieldBase : ItemBase, IReusableComponent<IField>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldBase"/> class.
        /// </summary>
        protected FieldBase()
        {
            this.DefaultValidators = new List<IValidator>();
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
        /// Gets the default validators.
        /// </summary>
        /// <value>
        /// The default validators.
        /// </value>
        public virtual IList<IValidator> DefaultValidators { get; private set; }

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
        /// Gets or sets the placeholder.
        /// </summary>
        /// <value>
        /// The placeholder.
        /// </value>
        [SitecoreDictionaryFallbackField("Placeholder", "Placeholder Text")]
        public virtual string Placeholder { get; set; }

        /// <summary>
        /// Gets or sets the dependent field.
        /// </summary>
        /// <value>
        /// The dependent field.
        /// </value>
        [SitecoreField("Dependent Field", Setting = SitecoreFieldSettings.InferType)]
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
        /// Gets or sets a value indicating whether this instance is hidden.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is hidden; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets the reusable component.
        /// </summary>
        /// <value>
        /// The reusable component.
        /// </value>
        [SitecoreField("Field", Setting = SitecoreFieldSettings.InferType)]
        public virtual IField ReusableComponent { get; set; }
        
        /// <summary>
        /// Gets or sets the reusable component.
        /// </summary>
        /// <value>
        /// The reusable component.
        /// </value>
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
    }
}
