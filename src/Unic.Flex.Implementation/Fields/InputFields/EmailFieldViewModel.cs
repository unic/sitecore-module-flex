namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Unic.Flex.Model.ViewModels.Fields.InputFields;

    /// <summary>
    /// Email field view model
    /// </summary>
    public class EmailFieldViewModel : InputFieldViewModel<string>
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
                return "Fields/InputFields/Email";
            }
        }

        /// <summary>
        /// Binds the needed attributes and properties after converting from domain model to the view model
        /// </summary>
        public override void BindProperties()
        {
            base.BindProperties();

            this.AddCssClass("flex_singletextfield");

            this.Attributes.Add("aria-multiline", false);
            this.Attributes.Add("role", "textbox");
            this.Attributes.Add("type", "email");
        }
    }
}
