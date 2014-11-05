namespace Unic.Flex.Model.ViewModel.Fields.ListFields
{
    using System.Collections.Generic;
    using Unic.Flex.Model.DataProviders;

    /// <summary>
    /// Base for all list field view models.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public abstract class ListFieldViewModel<TValue> : FieldBaseViewModel<TValue>
    {
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public IList<ListItem> Items { get; set; }
    }
}
