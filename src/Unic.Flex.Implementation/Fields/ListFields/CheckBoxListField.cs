namespace Unic.Flex.Implementation.Fields.ListFields
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using System.Linq;
    using Newtonsoft.Json.Linq;
    using Unic.Flex.Model.DataProviders;
    using Unic.Flex.Model.DomainModel.Fields.ListFields;

    /// <summary>
    /// Checkbox list field
    /// </summary>
    [SitecoreType(TemplateId = "{7532F52A-8BA7-4903-9B27-9E18FA1C4B92}")]
    public class CheckBoxListField : MulticheckListField<string[], ListItem>
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
    }
}
