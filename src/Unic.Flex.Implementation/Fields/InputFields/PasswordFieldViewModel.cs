namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Unic.Flex.Model.ViewModels.Fields.InputFields;

    /// <summary>
    /// Password field view model
    /// </summary>
    public class PasswordFieldViewModel : InputFieldViewModel<string>
    {
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
                return "Fields/InputFields/Password";
            }
        }

        /// <summary>
        /// Binds the properties.
        /// </summary>
        public override void BindProperties()
        {
            base.BindProperties();

            this.Attributes.Add("aria-multiline", false);
            this.Attributes.Add("role", "textbox");
            this.AddCssClass("flex_singletextfield");
            this.AddCssClass("flex_showpassword");
        }
    }
}
