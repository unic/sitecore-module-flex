namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Unic.Flex.Model.ViewModel.Fields.InputFields;

    /// <summary>
    /// Singletext field
    /// </summary>
    public class SinglelineTextFieldViewModel : InputFieldViewModel<string>
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
                return "Fields/InputFields/SinglelineText";
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
            this.AddCssClass("fm_singletextfield");
        }
    }
}
