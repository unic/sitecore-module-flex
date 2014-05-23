namespace Unic.Flex.Model.DomainModel.Sections
{
    using System.Collections.Generic;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.DomainModel.Fields;
    using Unic.Flex.Model.Presentation;

    /// <summary>
    /// The standard section.
    /// </summary>
    [SitecoreType(TemplateId = "{B2B5CAB2-2BD7-4FFE-80B1-7607A310771E}")]
    public class StandardSection : SectionBase, IPresentationComponent
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
        [SitecoreChildren(IsLazy = true, InferType = true)]
        public virtual IEnumerable<IField> Fields { get; set; }

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
                return "Sections/StandardSection";
            }
        }
    }
}
