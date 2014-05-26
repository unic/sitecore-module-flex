namespace Unic.Flex.Model.ViewModel.Fields.InputFields
{
    /// <summary>
    /// Multiline textfield (also called text area)
    /// </summary>
    public class MultilineTextFieldViewModel : InputFieldViewModel<string>
    {
        /// <summary>
        /// Gets or sets the number of rows.
        /// </summary>
        /// <value>
        /// The number of rows.
        /// </value>
        public virtual string Rows { get; set; }
    }
}
