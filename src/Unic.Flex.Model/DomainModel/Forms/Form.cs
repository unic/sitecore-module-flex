namespace Unic.Flex.Model.DomainModel.Forms
{
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Glass.Mapper.Sc.Fields;
    using System.Collections.Generic;
    using System.Linq;
    using Unic.Flex.Model.DomainModel.Fields;
    using Unic.Flex.Model.DomainModel.Plugs.LoadPlugs;
    using Unic.Flex.Model.DomainModel.Plugs.SavePlugs;
    using Unic.Flex.Model.DomainModel.Sections;
    using Unic.Flex.Model.DomainModel.Steps;
    using Unic.Flex.Model.GlassExtensions.Attributes;

    /// <summary>
    /// The complete form domain model object
    /// </summary>
    [SitecoreType(TemplateId = "{3AFE4256-1C3E-4441-98AF-B3D0037A8B1F}")]
    public class Form : ItemBase
    {
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
        public virtual IEnumerable<StepBase> Steps { get; set; }

        /// <summary>
        /// Gets or sets the load plugs.
        /// </summary>
        /// <value>
        /// The load plugs.
        /// </value>
        //// todo: plugs should be loaded also if no language version is available
        [SitecoreQuery("./Load Plugs/*", IsLazy = true, IsRelative = true, InferType = true)]
        public virtual IEnumerable<ILoadPlug> LoadPlugs { get; set; }

        /// <summary>
        /// Gets or sets the save plugs.
        /// </summary>
        /// <value>
        /// The save plugs.
        /// </value>
        //// todo: plugs should be loaded also if no language version is available
        [SitecoreQuery("./Save Plugs/*", IsLazy = true, IsRelative = true, InferType = true)]
        public virtual IEnumerable<ISavePlug> SavePlugs { get; set; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public virtual string Id
        {
            get
            {
                return this.ItemId.ToString();
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

        /// <summary>
        /// Gets all the real sections.
        /// </summary>
        /// <param name="stepNumber">The step number.</param>
        /// <returns>All real sections, for all steps or only for one</returns>
        public virtual IEnumerable<StandardSection> GetSections(int stepNumber = 0)
        {
            var steps = this.Steps;
            if (stepNumber > 0)
            {
                steps = steps.Where(step => step.StepNumber == stepNumber);
            }

            return steps.Where(step => !(step is Summary)).SelectMany(s => s.GetSections());
        }
    }
}
