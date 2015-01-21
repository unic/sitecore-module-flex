namespace Unic.Flex.Implementation.Fields.ListFields
{
    using System.Linq;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.DataProviders;
    using Unic.Flex.Model.DomainModel.Fields.ListFields;

    /// <summary>
    /// Radio button list field
    /// </summary>
    [SitecoreType(TemplateId = "{188683AC-127C-45A1-8F7D-4ABF6E68E7AF}")]
    public class RadioButtonListField : ListField<string, ListItem>
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
