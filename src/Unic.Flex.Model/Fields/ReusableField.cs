namespace Unic.Flex.Model.Fields
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.Components;

    /// <summary>
    /// Reusable field model.
    /// </summary>
    [SitecoreType(TemplateId = "{C71F2A06-56B1-4045-A96E-FDF1F3E76284}")]
    public class ReusableField : FieldBase<object>, IInvalidComponent
    {
        /// <summary>
        /// Gets or sets whether to show a component in summary.
        /// </summary>
        /// <value>
        /// The show in summary.
        /// </value>
        [SitecoreField("Show in Summary")]
        public override bool ShowInSummary { get; set; }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        /// <exception cref="System.NotImplementedException">This field has no view</exception>
        [SitecoreIgnore]
        public override string ViewName
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
