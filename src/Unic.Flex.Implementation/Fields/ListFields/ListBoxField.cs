namespace Unic.Flex.Implementation.Fields.ListFields
{
    using System.Linq;
    using System.Web.Mvc;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.DomainModel.Fields.ListFields;

    /// <summary>
    /// Listbox list field
    /// </summary>
    [SitecoreType(TemplateId = "{9A0208D4-4A39-436F-90E2-84765CC7A63A}")]
    public class ListBoxField : ListField<string[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxField"/> class.
        /// </summary>
        public ListBoxField()
        {
            //// todo: this values should be provided from Sitecore
            
            this.Items.Add(new SelectListItem { Text = "Apple", Value = "apple_value", Selected = true });
            this.Items.Add(new SelectListItem { Text = "Oranges", Value = "oranges_value", Selected = true });
        }

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public override string[] DefaultValue
        {
            get
            {
                return this.Items.Where(item => item.Selected).Select(item => item.Value).ToArray();
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        protected override void SetValue(object value)
        {
            if (value is string)
            {
                base.SetValue(value.ToString().Split(','));
                return;
            }

            base.SetValue(value);
        }
    }
}
