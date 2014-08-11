namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Unic.Flex.Model.ViewModel.Fields.InputFields;

    /// <summary>
    /// View model for a number field
    /// </summary>
    public class NumberFieldViewModel : InputFieldViewModel<int?>
    {
        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        /// <value>
        /// The minimum value.
        /// </value>
        public virtual int MinValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        /// <value>
        /// The maximum value.
        /// </value>
        public virtual int MaxValue { get; set; }

        /// <summary>
        /// Gets or sets the step.
        /// </summary>
        /// <value>
        /// The step.
        /// </value>
        public virtual int Step { get; set; }

        /// <summary>
        /// Binds the needed attributes and properties after converting from domain model to the view model
        /// </summary>
        public override void BindProperties()
        {
            base.BindProperties();

            this.Attributes.Add("type", "number");
            this.Attributes.Add("min", this.MinValue);

            if (this.MaxValue > this.MinValue)
            {
                this.Attributes.Add("max", this.MaxValue);
            }

            if (this.Step > 0)
            {
                this.Attributes.Add("step", this.Step);
            }
        }
    }
}
