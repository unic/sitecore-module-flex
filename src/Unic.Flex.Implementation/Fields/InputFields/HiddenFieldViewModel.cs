namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Unic.Flex.Model.ViewModels.Fields.InputFields;

    /// <summary>
    /// View model for a hidden field
    /// </summary>
    public class HiddenFieldViewModel : InputFieldViewModel<string>
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
                return "Fields/InputFields/Hidden";
            }
        }
    }
}
