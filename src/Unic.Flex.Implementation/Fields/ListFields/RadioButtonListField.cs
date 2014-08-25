namespace Unic.Flex.Implementation.Fields.ListFields
{
    using System.Web.Mvc;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.DomainModel.Fields.ListFields;

    [SitecoreType(TemplateId = "{188683AC-127C-45A1-8F7D-4ABF6E68E7AF}")]
    public class RadioButtonListField : ListField<string>
    {
        public RadioButtonListField()
        {
            // todo: this values should be provided from Sitecore

            this.Items.Add(new SelectListItem { Text = "Male", Value = "male_value" });
            this.Items.Add(new SelectListItem { Text = "Female", Value = "female_value" });
        }
    }
}
