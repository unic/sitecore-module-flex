namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Unic.Flex.Model.ViewModel.Fields.InputFields;

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
    }
}
