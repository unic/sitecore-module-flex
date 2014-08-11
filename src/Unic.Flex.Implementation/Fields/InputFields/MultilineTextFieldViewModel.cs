namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Unic.Flex.Model.ViewModel.Fields.InputFields;

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

        /// <summary>
        /// Binds the needed attributes and properties after converting from domain model to the view model
        /// </summary>
        public override void BindProperties()
        {
            base.BindProperties();

            this.Attributes.Add("Rows", this.Rows);
        }
    }
}
