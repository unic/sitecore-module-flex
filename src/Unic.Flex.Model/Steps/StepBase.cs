namespace Unic.Flex.Model.Steps
{
    using System.Collections.Generic;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.DomainModel;
    using Unic.Flex.Model.DomainModel.Sections;
    using Unic.Flex.Model.GlassExtensions.Attributes;
    using Unic.Flex.Model.Presentation;

    /// <summary>
    /// Base class for all steps.
    /// </summary>
    public abstract class StepBase : ItemBase, IPresentationComponent
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
        /// Gets or sets the button text.
        /// </summary>
        /// <value>
        /// The button text.
        /// </value>
        [SitecoreField("Button Text")]
        public virtual string ButtonText { get; set; }

        /// <summary>
        /// Gets or sets the sections.
        /// </summary>
        /// <value>
        /// The sections.
        /// </value>
        [SitecoreReusableChildren(IsLazy = true, InferType = true)]
        public virtual IEnumerable<StandardSection> Sections { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this step is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this step is active; otherwise, <c>false</c>.
        /// </value>
        [SitecoreIgnore]
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the step number.
        /// </summary>
        /// <value>
        /// The step number.
        /// </value>
        [SitecoreIgnore]
        public virtual int StepNumber { get; set; }

        /// <summary>
        /// Gets or sets the cancel URL.
        /// </summary>
        /// <value>
        /// The cancel URL.
        /// </value>
        [SitecoreIgnore]
        public virtual string CancelUrl { get; set; }

        /// <summary>
        /// Gets or sets the cancel text.
        /// </summary>
        /// <value>
        /// The cancel text.
        /// </value>
        [SitecoreIgnore]
        public virtual string CancelText { get; set; }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        [SitecoreIgnore]
        public abstract string ViewName { get; }
    }
}
