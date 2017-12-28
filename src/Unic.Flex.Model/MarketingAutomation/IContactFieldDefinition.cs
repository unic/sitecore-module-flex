namespace Unic.Flex.Model.MarketingAutomation
{
    using Glass.Mapper.Sc.Configuration.Attributes;

    [SitecoreType(TemplateId = "{D2FF6DBF-67AE-4387-B02A-1956E07DF809}", Cachable = false)]
    public class ContactFieldDefinition : ItemBase
    {
        [SitecoreField(FieldName = "Field Name")]
        public string FieldName { get; set; }
        
        [SitecoreParent(InferType = false)]
        public ContactFacetDefinition Facet { get; set; }
    }
}