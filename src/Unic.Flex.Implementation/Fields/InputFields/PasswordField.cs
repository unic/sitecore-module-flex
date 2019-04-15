namespace Unic.Flex.Implementation.Fields.InputFields
{
    using System;
    using Core.Context;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Sitecore.Configuration;
    using Unic.Flex.Implementation.Validators;
    using Unic.Flex.Model.Fields.InputFields;

    /// <summary>
    /// Password field
    /// </summary>
    [SitecoreType(TemplateId = "{47632CE8-1AA6-4A08-8A0B-528CBAE8C07E}")]
    public class PasswordField : InputField<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordField"/> class.
        /// </summary>
        public PasswordField(IFlexContext context)
        {
            // we use the standard values item here as validator id, because we do not have a specific item for the validator as it is defined here and not in Sitecore
            this.DefaultValidators.Add(new PasswordStrengthValidator(context) { ValidatorId = new Guid(Settings.GetSetting("Flex.PasswordStrengthValidator.StandardValues.ItemId")) });
        }

        /// <summary>
        /// Gets the text value.
        /// </summary>
        /// <value>
        /// The text value.
        /// </value>
        [SitecoreIgnore]
        public override string TextValue
        {
            get
            {
                return "********";
            }
        }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        [SitecoreIgnore]
        public override string ViewName
        {
            get
            {
                return "Fields/InputFields/Password";
            }
        }

        /// <summary>
        /// Binds the properties.
        /// </summary>
        public override void BindProperties()
        {
            base.BindProperties();

            this.Attributes.Add("aria-multiline", "false");
            this.Attributes.Add("role", "textbox");
            this.AddCssClass("flex_singletextfield");
            this.AddCssClass("flex_showpassword");
        }
    }
}
