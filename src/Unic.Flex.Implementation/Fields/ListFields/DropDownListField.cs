namespace Unic.Flex.Implementation.Fields.ListFields
{
    using System.Linq;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.DomainModel.Fields.ListFields;

    /// <summary>
    /// Dropdown list field
    /// </summary>
    [SitecoreType(TemplateId = "{18C0BDC1-5162-4CE4-A92A-0C9A8CAFCF11}")]
    public class DropDownListField : ListField<string>
    {
        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public override string DefaultValue
        {
            get
            {
                var selectedItem = this.Items.FirstOrDefault(item => item.Selected);
                return selectedItem != null ? selectedItem.Value : string.Empty;
            }
        }
    }
}
