namespace Unic.Flex.Model.DomainModel.Fields
{
    /// <summary>
    /// Base class for fields with a value
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public abstract class FieldBase<TValue> : FieldBase
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public virtual TValue Value { get; set; }
    }
}
