namespace Unic.Flex.Model.ViewModel.Fields.InputFields
{
    /// <summary>
    /// View model used for the honeypot spam protection
    /// </summary>
    public class HoneypotFieldViewModel : InputFieldViewModel<string>
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
                return "Fields/InputFields/Honeypot";
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
            this.AddCssClass("flex_singletextfield info3-block");
        }
    }
}
