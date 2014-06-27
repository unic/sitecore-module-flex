namespace Unic.Flex.Model.DomainModel.Fields
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
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
                this.Value = value != null ? (TValue)value : default(TValue);
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
    }

    /// <summary>
    /// Abstract base class for all fields
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public abstract class FieldBase : ItemBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldBase"/> class.
        /// </summary>
        protected FieldBase()
        {
            this.DefaultValidators = new List<IValidator>();
        }
        
        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public abstract string ViewName { get; }

        /// <summary>
        /// Gets the CSS class.
        /// </summary>
        /// <value>
        /// The CSS class.
        /// </value>
        public virtual string CssClass
        {
            get
            {
                return string.Empty;
            }
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
        [SitecoreSharedField(FieldName = "Css Class")]
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
    }
}
