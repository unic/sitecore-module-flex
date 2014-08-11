namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Unic.Flex.Model.ViewModel.Fields.InputFields;

    /// <summary>
    /// Singletext field
    /// </summary>
    public class SinglelineTextFieldViewModel : InputFieldViewModel<string>
    {
        /// <summary>
        /// Binds the properties.
        /// </summary>
        public override void BindProperties()
        {
            base.BindProperties();

            this.Attributes.Add("aria-multiline", false);
            this.Attributes.Add("role", "textbox");
            this.CssClass = "fm_singletextfield";
        }
    }
}
