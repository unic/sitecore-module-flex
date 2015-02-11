namespace Unic.Flex.Model.DataProviders
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for a provider which provides items for a list field.
    /// </summary>
    /// <typeparam name="TType">The type of the data items.</typeparam>
    public interface IDataProvider<out TType> where TType : IDataItem
    {
        /// <summary>
        /// Gets the items for the list field.
        /// </summary>
        /// <returns>List of items</returns>
        IEnumerable<TType> GetItems();

        /// <summary>
        /// Gets the text value for a given value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The text value
        /// </returns>
        string GetTextValue<TValue>(object value);
    }
}
