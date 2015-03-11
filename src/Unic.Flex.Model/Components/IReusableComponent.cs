namespace Unic.Flex.Model.Components
{
    /// <summary>
    /// Reusable component
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public interface IReusableComponent<TValue> : IReusableComponent
    {
        /// <summary>
        /// Gets or sets the reusable component.
        /// </summary>
        /// <value>
        /// The reusable component.
        /// </value>
        new TValue ReusableComponent { get; set; }
    }

    /// <summary>
    /// Non-generic reusable component
    /// </summary>
    public interface IReusableComponent
    {
        /// <summary>
        /// Gets or sets the reusable component.
        /// </summary>
        /// <value>
        /// The reusable component.
        /// </value>
        object ReusableComponent { get; set; }
    }
}
