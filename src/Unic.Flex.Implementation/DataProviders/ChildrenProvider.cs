namespace Unic.Flex.Implementation.DataProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.DataProviders;

    /// <summary>
    /// Provider for list field items which lists all child items.
    /// </summary>
    [SitecoreType(TemplateId = "{66926539-DC85-4A9F-BD60-8A950DD842B7}")]
    public class ChildrenProvider : IDataProvider<ListItem>
    {
        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        [SitecoreChildren]
        public virtual IEnumerable<ListItem> Children { get; set; }

        /// <summary>
        /// Gets the items for the list field.
        /// </summary>
        /// <returns>
        /// List of items
        /// </returns>
        public virtual IEnumerable<ListItem> GetItems()
        {
            return this.Children;
        }

        /// <summary>
        /// Gets the text value for a given value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The text value
        /// </returns>
        public virtual string GetTextValue<TValue>(object value)
        {
            if (object.Equals(value, null)) return string.Empty;
            if (typeof(TValue) == typeof(string[])) return this.GetTextValue(value as string[]);
            if (typeof(TValue) == typeof(string)) return this.GetTextValue(value as string);

            return string.Empty;
        }

        /// <summary>
        /// Gets the text value from a simple value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Text value of the field</returns>
        private string GetTextValue(string value)
        {
            var selectedItem = this.Children.FirstOrDefault(item => item.Value == value);
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
                this.Children.Where(item => !string.IsNullOrWhiteSpace(item.Value))
                    .Where(item => value.Contains(item.Value))
                    .Select(item => item.Text));
        }
    }
}
