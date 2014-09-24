namespace Unic.Flex.Implementation.Fields.ListFields
{
    using System.Linq;
    using System.Web.Mvc;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.DomainModel.Fields.ListFields;

    /// <summary>
    /// Checkbox list field
    /// </summary>
    [SitecoreType(TemplateId = "{7532F52A-8BA7-4903-9B27-9E18FA1C4B92}")]
    public class CheckBoxListField : MulticheckListField<string[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBoxListField"/> class.
        /// </summary>
        public CheckBoxListField()
        {
            //// todo: this values should be provided from Sitecore

            this.Items.Add(new SelectListItem { Text = "Red", Value = "red_value" });
            this.Items.Add(new SelectListItem { Text = "Green", Value = "green_value", Selected = true });
            this.Items.Add(new SelectListItem { Text = "Blue", Value = "blue_value", Selected = true });
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
