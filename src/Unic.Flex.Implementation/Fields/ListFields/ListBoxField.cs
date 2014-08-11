namespace Unic.Flex.Implementation.Fields.ListFields
{
    using System.Web.Mvc;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.DomainModel.Fields.ListFields;

    [SitecoreType(TemplateId = "{9A0208D4-4A39-436F-90E2-84765CC7A63A}")]
    public class ListBoxField : ListField<string[]>
    {
        public ListBoxField()
        {
            // todo: this values should be provided from Sitecore
            
            this.Items.Add(new SelectListItem { Text = "Please Select", Value = string.Empty });
            this.Items.Add(new SelectListItem { Text = "Apple", Value = "apple_value" });
            this.Items.Add(new SelectListItem { Text = "Oranges", Value = "oranges_value" });
        }
    }
}
