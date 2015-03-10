namespace Unic.Flex.Implementation.Fields.ListFields
{
    using System.Linq;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Newtonsoft.Json.Linq;
    using Unic.Flex.Model.DataProviders;
    using Unic.Flex.Model.Fields.ListFields;

    /// <summary>
    /// Listbox list field
    /// </summary>
    [SitecoreType(TemplateId = "{9A0208D4-4A39-436F-90E2-84765CC7A63A}")]
    public class ListBoxField : ListField<string[], ListItem>
    {
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

            var jsonValue = value as JArray;
            if (jsonValue != null)
            {
                base.SetValue(jsonValue.Select(token => token.ToString()).ToArray());
                return;
            }

            base.SetValue(value);
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
                return "Fields/ListFields/ListBox";
            }
        }
    }
}
