namespace Unic.Flex.Model.DomainModel.Sections
{
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using System.Collections.Generic;
    using Unic.Flex.Model.DomainModel.Components;
    using Unic.Flex.Model.DomainModel.Fields;
    using Unic.Flex.Model.DomainModel.Steps;
    using Unic.Flex.Model.GlassExtensions.Attributes;

    /// <summary>
    /// The standard section.
    /// </summary>
    [SitecoreType(TemplateId = "{B2B5CAB2-2BD7-4FFE-80B1-7607A310771E}")]
    public class StandardSection : ItemBase, ITooltip, IVisibilityDependency, IReusableComponent<StandardSection>
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [SitecoreField("Title")]
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets the step title.
        /// </summary>
        /// <value>
        /// The step title.
        /// </value>
        public virtual string StepTitle
        {
            get
            {
                var step = this.Parent as StepBase;
                return step != null ? step.Title : string.Empty;
            }
        }

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
        public virtual IEnumerable<IField> Fields { get; set; }

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
        [SitecoreField("Dependent Field", Setting = SitecoreFieldSettings.InferType)]
        public virtual IField DependentField { get; set; }

        /// <summary>
        /// Gets or sets the dependent value.
        /// </summary>
        /// <value>
        /// The dependent value.
        /// </value>
        [SitecoreField("Dependent Value")]
        // todo: this must not be a simple single-line text field
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
                this.ReusableComponent = value as StandardSection;
            }
        }
    }
}
