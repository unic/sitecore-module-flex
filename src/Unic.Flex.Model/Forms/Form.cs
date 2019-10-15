namespace Unic.Flex.Model.Forms
{
    using System.Collections.Generic;
    using System.Linq;
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Glass.Mapper.Sc.Fields;
    using Specifications;
    using Unic.Flex.Model.GlassExtensions.Attributes;
    using Unic.Flex.Model.Plugs;
    using Unic.Flex.Model.Steps;

    /// <summary>
    /// The complete form domain model object
    /// </summary>
    [SitecoreType(TemplateId = "{3AFE4256-1C3E-4441-98AF-B3D0037A8B1F}")]
    public class Form : ItemBase, IForm
    {
        /// <summary>
        /// The active step
        /// </summary>
        private IStep activeStep;
        
        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        [SitecoreInfo(SitecoreInfoType.Language)]
        public virtual string Language { get; set; }
        
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [SitecoreField("Title")]
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets the title level.
        /// </summary>
        /// <value>
        /// The title level.
        /// </value>
        [SitecoreSharedField("Title Level")]
        public virtual Specification TitleLevel { get; set; }

        /// <summary>
        /// Gets or sets the title visually hidden.
        /// </summary>
        /// <value>
        /// The title visually hidden.
        /// </value>
        [SitecoreField("Title Visually Hidden")]
        public virtual bool TitleVisuallyHidden { get; set; }

        /// <summary>
        /// Gets or sets the introduction.
        /// </summary>
        /// <value>
        /// The introduction.
        /// </value>
        [SitecoreField("Introduction")]
        public virtual string Introduction { get; set; }

        /// <summary>
        /// Gets or sets the cancel link.
        /// </summary>
        /// <value>
        /// The cancel link.
        /// </value>
        [SitecoreField("Cancel Link")]
        public virtual Link CancelLink { get; set; }

        /// <summary>
        /// Gets or sets the success message.
        /// </summary>
        /// <value>
        /// The success message.
        /// </value>
        [SitecoreDictionaryFallbackField("Message", "Default success message")]
        public virtual string SuccessMessage { get; set; }

        /// <summary>
        /// Gets or sets the success redirect.
        /// </summary>
        /// <value>
        /// The success redirect.
        /// </value>
        [SitecoreField("Redirect")]
        public virtual ItemBase SuccessRedirect { get; set; }

        /// <summary>
        /// Gets or sets the autocomplete value.
        /// </summary>
        /// <value>
        /// The autocomplete value.
        /// </value>
        [SitecoreField("Autocomplete")]
        public virtual Specification Autocomplete { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        [SitecoreDictionaryFallbackField("Error Message", "Default error")]
        public virtual string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the steps.
        /// </summary>
        /// <value>
        /// The steps.
        /// </value>
        [SitecoreQuery("./Steps/*", IsLazy = true, IsRelative = true, InferType = true)]
        public virtual IEnumerable<IStep> Steps { get; set; }

        /// <summary>
        /// Gets or sets the load plugs.
        /// </summary>
        /// <value>
        /// The load plugs.
        /// </value>
        [SitecoreSharedQuery("./Load Plugs/*", IsLazy = false, IsRelative = true, InferType = true)]
        public virtual IEnumerable<ILoadPlug> LoadPlugs { get; set; }

        /// <summary>
        /// Gets or sets the save plugs.
        /// </summary>
        /// <value>
        /// The save plugs.
        /// </value>
        [SitecoreSharedQuery("./Save Plugs/*", IsLazy = false, IsRelative = true, InferType = true)]
        public virtual IEnumerable<ISavePlug> SavePlugs { get; set; }

        /// <summary>
        /// Gets the active step.
        /// </summary>
        /// <value>
        /// The active step.
        /// </value>
        [SitecoreIgnore]
        public virtual IStep ActiveStep
        {
            get
            {
                return this.activeStep ?? (this.activeStep = this.Steps.FirstOrDefault(step => step.IsActive) ?? this.Steps.FirstOrDefault());
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
                return "Form";
            }
        }
    }
}
