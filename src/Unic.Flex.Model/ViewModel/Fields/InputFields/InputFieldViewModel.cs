namespace Unic.Flex.Model.ViewModel.Fields.InputFields
{
    /// <summary>
    /// View model for basic input fields
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public abstract class InputFieldViewModel<TValue> : FieldBaseViewModel<TValue>
    {
        /// <summary>
        /// Gets or sets the placeholder.
        /// </summary>
        /// <value>
        /// The placeholder.
        /// </value>
        public virtual string Placeholder { get; set; }

        /// <summary>
        /// Binds the needed attributes and properties after converting from domain model to the view model
        /// </summary>
        public override void BindProperties()
        {
            base.BindProperties();

            if (!string.IsNullOrWhiteSpace(this.Placeholder))
            {
                this.Attributes.Add("placeholder", this.Placeholder);
            }
        }
    }
}
