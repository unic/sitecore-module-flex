namespace Unic.Flex.Model.DomainModel.Fields.ListFields
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Glass.Mapper.Sc.Configuration;
    using Unic.Flex.Model.DataProviders;
    using Unic.Flex.Model.DomainModel.Global;
    using Unic.Flex.Model.GlassExtensions.Attributes;
    using Unic.Flex.Model.SortOrder;

    /// <summary>
    /// Base class for all list fields.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public abstract class ListField<TValue> : FieldBase<TValue>
    {
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public virtual IList<ListItem> Items
        {
            get
            {
                // check for valid provider
                if (this.DataProvider == null) return new List<ListItem>();
                
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
        public virtual IDataProvider DataProvider { get; set; }

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
                var value = string.Empty;
                
                // check the current value
                if (object.Equals(this.Value, null)) return base.TextValue;
                if (typeof(TValue) == typeof(string[])) value = this.GetTextValue(this.Value as string[]);
                if (typeof(TValue) == typeof(string)) value = this.GetTextValue(this.Value as string);
                
                // return
                return !string.IsNullOrWhiteSpace(value) ? value : "-";
            }
        }

        /// <summary>
        /// Gets the text value from a simple value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Text value of the field</returns>
        private string GetTextValue(string value)
        {
            var selectedItem = this.Items.FirstOrDefault(item => item.Value == value);
            return selectedItem != null ? selectedItem.Text : string.Empty;
        }

        /// <summary>
        /// Gets the text value from a array value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Text value of the field.</returns>
        private string GetTextValue(IEnumerable<string> value)
        {
            return string.Join(
                Environment.NewLine,
                this.Items.Where(item => !string.IsNullOrWhiteSpace(item.Value))
                    .Where(item => value.Contains(item.Value))
                    .Select(item => item.Text));
        }
    }
}
