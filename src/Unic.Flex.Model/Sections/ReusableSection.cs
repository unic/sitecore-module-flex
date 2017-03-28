namespace Unic.Flex.Model.Sections
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.Components;

    /// <summary>
    /// The reusable section.
    /// </summary>
    [SitecoreType(TemplateId = "{773FF85E-C512-4825-A4EA-1FFB181525C9}")]
    public class ReusableSection : StandardSection, IInvalidComponent
    {
        /// <summary>
        /// Gets or sets whether to show a component in summary.
        /// </summary>
        /// <value>
        /// The show in summary.
        /// </value>
        [SitecoreField("Show in Summary")]
        public override bool ShowInSummary { get; set; }
    }
}
