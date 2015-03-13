namespace Unic.Flex.Model.Fields.InputFields
{
    using Unic.Flex.Model.GlassExtensions.Attributes;

    /// <summary>
    /// Base class for simple input fields
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public abstract class InputField<TValue> : FieldBase<TValue>
    {
        /// <summary>
        /// Gets or sets the placeholder.
        /// </summary>
        /// <value>
        /// The placeholder.
        /// </value>
        [SitecoreDictionaryFallbackField("Placeholder", "Placeholder Text")]
        public virtual string Placeholder { get; set; }

        /// <summary>
        /// Binds the properties.
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
