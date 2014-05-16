namespace Unic.Flex.Model.Sections
{
    using Glass.Mapper.Sc.Configuration.Attributes;

    [SitecoreType(TemplateId = "{773FF85E-C512-4825-A4EA-1FFB181525C9}")]
    public class ReusableSection : SectionBase
    {
        [SitecoreField("Section")]
        public virtual StandardSection Section { get; set; }
    }
}
