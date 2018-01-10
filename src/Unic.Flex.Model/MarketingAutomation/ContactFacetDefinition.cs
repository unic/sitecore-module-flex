namespace Unic.Flex.Model.MarketingAutomation
{
    using Glass.Mapper.Sc.Configuration.Attributes;

    [SitecoreType(TemplateId = "{C0F3505F-7033-4700-A51F-9401565BC38A}", Cachable = false)]
    public class ContactFacetDefinition : ItemBase
    {
        [SitecoreField(FieldName = "Type")]
        public string Type { get; set; }
        
        [SitecoreField(FieldName = "Facet Name")]
        public string FacetName { get; set; }
    }
}