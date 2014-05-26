namespace Unic.Flex.Model.ViewModel.Fields.InputFields
{
    /// <summary>
    /// View model for a number field
    /// </summary>
    public class NumberFieldViewModel : InputFieldViewModel<int>
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
    }
}
