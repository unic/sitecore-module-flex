namespace Unic.Flex.Implementation.Fields.ListFields
{
    using System.Collections.Generic;
    using System.Linq;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Core.Globalization;
    using Unic.Flex.Model.DataProviders;
    using Unic.Flex.Model.Fields.ListFields;

    /// <summary>
    /// Dropdown list field
    /// </summary>
    [SitecoreType(TemplateId = "{18C0BDC1-5162-4CE4-A92A-0C9A8CAFCF11}")]
    public class DropDownListField : ListField<string, ListItem>
    {
        /// <summary>
        /// Gets or sets a value indicating whether to add an empty option to the list.
        /// </summary>
        /// <value>
        ///   <c>true</c> if an empty option should be added; otherwise, <c>false</c>.
        /// </value>
        [SitecoreField("Add Empty Option")]
        public virtual bool AddEmptyOption { get; set; }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public override IList<ListItem> Items
        {
            get
            {
                var items = base.Items;
                if (this.AddEmptyOption)
                {
                    items.Insert(0, new ListItem { Text = TranslationHelper.FlexText("Please choose"), Value = string.Empty });
                }

                return items;
            }
        }

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

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public override string ViewName
        {
            get
            {
                return "Fields/ListFields/DropDownList";
            }
        }

        /// <summary>
        /// Binds the properties.
        /// </summary>
        public override void BindProperties()
        {
            base.BindProperties();

            this.AddCssClass("flex_singleselectfield");
        }
    }
}
