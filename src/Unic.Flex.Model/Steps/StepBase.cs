namespace Unic.Flex.Model.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using Definitions;
    using Fields;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.GlassExtensions.Attributes;
    using Unic.Flex.Model.Sections;

    /// <summary>
    /// Base class for all steps.
    /// </summary>
    public abstract class StepBase : ItemBase, IStep
    {
        /// <summary>
        /// The sections
        /// </summary>
        private IList<ISection> sections;
        
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
        public virtual IEnumerable<ISection> LazySections { private get; set; }
        
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
        /// Gets or sets the sections.
        /// </summary>
        /// <value>
        /// The sections.
        /// </value>
        [SitecoreIgnore]
        public virtual IList<ISection> Sections
        {
            get
            {
                return this.sections ?? (this.sections = this.LazySections.ToList());
            }

            set
            {
                this.sections = value;
            }
        }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        [SitecoreIgnore]
        public abstract string ViewName { get; }

        /// <summary>
        /// Gets the attributes that should be set on the step's submit button.
        /// </summary>
        [SitecoreIgnore]
        public virtual IDictionary<string, object> ButtonAttributes
        {
            get
            {
                var describingFields = this.Sections?.SelectMany(section => section.Fields)
                    .Where(field =>
                    {
                        var describingField = field as IStepDescribingField;
                        return describingField != null && describingField.DescribesFormStep;
                    })
                    .Select(field => string.Format("{0}_{1}", field.Id, Constants.DescriptorIdSuffix))
                    .ToList();
                if (describingFields == null || !describingFields.Any()) return new Dictionary<string, object>();
                return new Dictionary<string, object> { { "aria-describedby", string.Join(" ", describingFields) } };
            }
        }
    }
}
