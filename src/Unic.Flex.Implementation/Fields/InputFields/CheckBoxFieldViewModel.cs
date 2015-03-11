namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Unic.Flex.Model.ViewModels.Fields.InputFields;

    /// <summary>
    /// CheckBox view model
    /// </summary>
    public class CheckBoxFieldViewModel : InputFieldViewModel<bool>
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
                return "Fields/InputFields/CheckBox";
            }
        }

        /// <summary>
        /// Binds the needed attributes and properties after converting from domain model to the view model
        /// </summary>
        public override void BindProperties()
        {
            base.BindProperties();

            this.AddCssClass("flex_singlecheckbox");

            this.Attributes.Add("aria-multiline", false);
            this.Attributes.Add("aria-checked", this.Value);
            this.Attributes.Add("role", "checkbox");
        }
    }
}
