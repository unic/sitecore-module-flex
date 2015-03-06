namespace Unic.Flex.Model.DomainModel.Fields.ListFields
{
    using System.Collections.Generic;
    using System.Linq;
    using Glass.Mapper.Sc.Configuration;
    using Unic.Flex.Model.DataProviders;
    using Unic.Flex.Model.GlassExtensions.Attributes;
    using Unic.Flex.Model.SortOrder;
    using Unic.Flex.Model.Specifications;

    /// <summary>
    /// Base class for all list fields.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <typeparam name="TType">The type of the data items.</typeparam>
    public abstract class ListField<TValue, TType> : FieldBase<TValue> where TType : IDataItem
    {
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public virtual IList<TType> Items
        {
            get
            {
                // check for valid provider
                if (this.DataProvider == null) return new List<TType>();
                
                // get items
                var items = this.DataProvider.GetItems();

                // sort items
                if (this.ItemsSortOrder != null && !string.IsNullOrWhiteSpace(this.ItemsSortOrder.Value))
                {
                    var strategy = StrategyFactory.CreateInstance(this.ItemsSortOrder.Value);
                    if (strategy != null) items = strategy.Sort(items);
                }

                // return
                return items.ToList();
            }
        }

        /// <summary>
        /// Gets or sets the items provider.
        /// </summary>
        /// <value>
        /// The items provider.
        /// </value>
        [SitecoreSharedField("Data Provider", Setting = SitecoreFieldSettings.InferType)]
        public virtual IDataProvider<TType> DataProvider { get; set; }

        /// <summary>
        /// Gets or sets the items sort order.
        /// </summary>
        /// <value>
        /// The items sort order.
        /// </value>
        [SitecoreSharedField("Items Sort Order", Setting = SitecoreFieldSettings.InferType)]
        public virtual Specification ItemsSortOrder { get; set; }

        /// <summary>
        /// Gets the text value.
        /// </summary>
        /// <value>
        /// The text value.
        /// </value>
        public override string TextValue
        {
            get
            {
                var value = this.DataProvider != null
                                ? this.DataProvider.GetTextValue<TValue>(this.Value)
                                : string.Empty;
                
                return !string.IsNullOrWhiteSpace(value) ? value : "-";
            }
        }
    }
}
