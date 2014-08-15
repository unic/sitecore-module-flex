namespace Unic.Flex.Implementation.Fields.ListFields
{
    using System.Linq;
    using System.Web.Mvc;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.DomainModel.Fields.ListFields;

    [SitecoreType(TemplateId = "{7532F52A-8BA7-4903-9B27-9E18FA1C4B92}")]
    public class CheckBoxListField : MulticheckListField<string[]>
    {
        public CheckBoxListField()
        {
            // todo: this values should be provided from Sitecore

            this.Items.Add(new SelectListItem { Text = "Red", Value = "red_value" });
            this.Items.Add(new SelectListItem { Text = "Green", Value = "green_value" });
            this.Items.Add(new SelectListItem { Text = "Blue", Value = "blue_value" });
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
