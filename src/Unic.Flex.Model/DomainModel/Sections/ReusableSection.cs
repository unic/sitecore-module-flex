namespace Unic.Flex.Model.DomainModel.Sections
{
    using Glass.Mapper.Sc.Configuration.Attributes;

    /// <summary>
    /// The reusable section.
    /// </summary>
    [SitecoreType(TemplateId = "{773FF85E-C512-4825-A4EA-1FFB181525C9}")]
    public class ReusableSection : SectionBase
    {
        /// <summary>
        /// Gets or sets the referenced section.
        /// </summary>
        /// <value>
        /// The referenced section.
        /// </value>
        [SitecoreField("Section")]
        public virtual StandardSection Section { get; set; }
    }
}
