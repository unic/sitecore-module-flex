namespace Unic.Flex.Implementation.Fields.ListFields
{
    using System.Web.Mvc;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.DomainModel.Fields.ListFields;

    /// <summary>
    /// Dropdown list field
    /// </summary>
    [SitecoreType(TemplateId = "{18C0BDC1-5162-4CE4-A92A-0C9A8CAFCF11}")]
    public class DropDownListField : ListField<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DropDownListField"/> class.
        /// </summary>
        public DropDownListField()
        {
            //// todo: this values should be provided from Sitecore
            
            this.Items.Add(new SelectListItem { Text = "Please Select", Value = string.Empty });
            this.Items.Add(new SelectListItem { Text = "Male", Value = "male_value" });
            this.Items.Add(new SelectListItem { Text = "Female", Value = "female_value" });
        }
    }
}
