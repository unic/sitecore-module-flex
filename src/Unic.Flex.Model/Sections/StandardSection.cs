namespace Unic.Flex.Model.Sections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.Components;
    using Unic.Flex.Model.Fields;
    using Unic.Flex.Model.GlassExtensions.Attributes;
    using Unic.Flex.Model.Steps;

    /// <summary>
    /// The standard section.
    /// </summary>
    [SitecoreType(TemplateId = "{B2B5CAB2-2BD7-4FFE-80B1-7607A310771E}")]
    public class StandardSection : ItemBase, ISection, IReusableComponent<StandardSection>
    {
        /// <summary>
        /// Private field for storing the is hidden property.
        /// </summary>
        private bool? isHidden;

        /// <summary>
        /// The fields
        /// </summary>
        private IList<IField> fields;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardSection"/> class.
        /// </summary>
        public StandardSection()
        {
            this.ContainerAttributes = new Dictionary<string, object>();
        }
        
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [SitecoreField("Title")]
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        [SitecoreParent(IsLazy = true, InferType = true)]
        public virtual ItemBase Step { private get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable the fieldset markup.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the fieldset markup should not be outputed; otherwise, <c>false</c>.
        /// </value>
        [SitecoreField("Disable Fieldset")]
        public virtual bool DisableFieldset { get; set; }

        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        [SitecoreReusableChildren(IsLazy = true, InferType = true)]
        public virtual IEnumerable<IField> LazyFields { private get; set; }

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
        [SitecoreField("Section")]
        public virtual StandardSection ReusableComponent { get; set; }

        /// <summary>
        /// Gets the step title.
        /// </summary>
        /// <value>
        /// The step title.
        /// </value>
        [SitecoreIgnore]
        public virtual string StepTitle
        {
            get
            {
                var step = this.Step as IStep;
                return step != null ? step.Title : string.Empty;
            }
        }

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

                // get the value of the dependent field
                var dependentValue = this.DependentField.Value != null ? this.DependentField.Value.ToString() : string.Empty;
                var listValue = this.DependentField.Value as IEnumerable<string>;
                if (listValue != null) dependentValue = string.Join(",", listValue);

                // compare the values
                this.isHidden = !dependentValue.Equals(this.DependentValue, StringComparison.InvariantCultureIgnoreCase);

                // mark sections with only hidden fields also as hidden
                if (!this.isHidden.Value && this.Fields.All(f => f.IsHidden))
                {
                    this.isHidden = true;
                }

                return this.isHidden.Value;
            }
        }

        /// <summary>
        /// Gets the sections.
        /// </summary>
        /// <value>
        /// The sections.
        /// </value>
        [SitecoreIgnore]
        public virtual IList<IField> Fields
        {
            get
            {
                return this.fields ?? (this.fields = this.LazyFields.ToList());
            }
        }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        [SitecoreIgnore]
        public virtual string ViewName
        {
            get
            {
                return "Sections/StandardSection";
            }
        }

        /// <summary>
        /// Gets or sets the tooltip.
        /// </summary>
        /// <value>
        /// The tooltip.
        /// </value>
        [SitecoreIgnore]
        public virtual Tooltip Tooltip { get; set; }

        /// <summary>
        /// Gets the container attributes.
        /// </summary>
        /// <value>
        /// The container attributes.
        /// </value>
        [SitecoreIgnore]
        public virtual IDictionary<string, object> ContainerAttributes { get; private set; }

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
                this.ReusableComponent = value as StandardSection;
            }
        }

        /// <summary>
        /// Binds the properties.
        /// </summary>
        public virtual void BindProperties()
        {
            // handle field dependency
            if (this.DependentField != null)
            {
                this.ContainerAttributes.Add("data-flexform-dependent", "{" + HttpUtility.HtmlEncode(string.Format("\"from\": \"{0}\", \"value\": \"{1}\"", this.DependentField.Id, this.DependentValue)) + "}");
            }
        }
    }
}
