namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Unic.Flex.Model.ViewModel.Fields.InputFields;

    /// <summary>
    /// View model for a number field
    /// </summary>
    public class NumberFieldViewModel : InputFieldViewModel<decimal?>
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
                return "Fields/InputFields/Number";
            }
        }

        /// <summary>
        /// Gets or sets the step.
        /// </summary>
        /// <value>
        /// The step.
        /// </value>
        public virtual string Step { get; set; }

        /// <summary>
        /// Binds the needed attributes and properties after converting from domain model to the view model
        /// </summary>
        public override void BindProperties()
        {
            base.BindProperties();

            this.AddCssClass("flex_singletextfield");

            this.Attributes.Add("aria-multiline", false);
            this.Attributes.Add("role", "textbox");
            this.Attributes.Add("type", "number");

            if (!string.IsNullOrWhiteSpace(this.Step))
            {
                this.Attributes.Add("step", this.Step);    
            }
        }
    }
}
