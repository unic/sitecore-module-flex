namespace Unic.Flex.Model.Forms
{
    using System.Collections.Generic;
    using System.Linq;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.Presentation;
    using Unic.Flex.Model.Steps;

    /// <summary>
    /// The complete form domain model object
    /// </summary>
    [SitecoreType(TemplateId = "{3AFE4256-1C3E-4441-98AF-B3D0037A8B1F}")]
    public class Form : ItemBase, IPresentationComponent
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
        /// Gets or sets the introduction.
        /// </summary>
        /// <value>
        /// The introduction.
        /// </value>
        [SitecoreField("Introduction")]
        public virtual string Introduction { get; set; }

        /// <summary>
        /// Gets or sets the steps.
        /// </summary>
        /// <value>
        /// The steps.
        /// </value>
        [SitecoreQuery("./Steps/*", IsLazy = true, IsRelative = true, InferType = true)]
        public virtual IEnumerable<StepBase> Steps { get; set; }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public virtual string ViewName
        {
            get
            {
                return "Form";
            }
        }

        /// <summary>
        /// Gets the active step.
        /// </summary>
        /// <returns>The first step set as active or the first step if no active step is found</returns>
        public virtual StepBase GetActiveStep()
        {
            return this.Steps.FirstOrDefault(step => step.IsActive) ?? this.Steps.FirstOrDefault();
        }
    }
}
