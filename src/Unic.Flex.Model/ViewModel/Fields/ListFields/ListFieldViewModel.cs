namespace Unic.Flex.Model.ViewModel.Fields.ListFields
{
    using System.Collections.Generic;
    using Unic.Flex.Model.DataProviders;

    /// <summary>
    /// Base for all list field view models.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <typeparam name="TType">The type of the data item.</typeparam>
    public abstract class ListFieldViewModel<TValue, TType> : FieldBaseViewModel<TValue> where TType : IDataItem
    {
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public virtual IList<TType> Items { get; set; }
    }
}
