namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Unic.Flex.Model.ViewModel.Fields.InputFields;

    /// <summary>
    /// Email field view model
    /// </summary>
    public class EmailFieldViewModel : InputFieldViewModel<string>
    {
        /// <summary>
        /// Binds the needed attributes and properties after converting from domain model to the view model
        /// </summary>
        public override void BindProperties()
        {
            base.BindProperties();

            this.Attributes.Add("type", "email");
        }
    }
}
