namespace Unic.Flex.Model.GlassExtensions.Handlers
{
    using Glass.Mapper;
    using Glass.Mapper.Sc.DataMappers;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Sections;
    using Unic.Flex.Model.Components;
    using Unic.Flex.Model.GlassExtensions.Configurations;

    /// <summary>
    /// Type mapper for reusable children
    /// </summary>
    public class SitecoreReusableChildrenTypeMapper : SitecoreChildrenMapper
    {
        /// <summary>
        /// Maps data from the CMS value to the .Net property value
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns>
        /// List with mapped children.
        /// </returns>
        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            return Utilities.CreateGenericType(
                typeof(ReusableComponentList<>),
                new[] { Utilities.GetGenericArgument(Configuration.PropertyInfo.PropertyType) },
                base.MapToProperty(mappingContext));
        }

        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if this instance can handle the specified configuration; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanHandle(Glass.Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is SitecoreReusableChildrenConfiguration;
        }

        /// <summary>
        /// Enumerable implemenetation to select the reusable component if available.
        /// </summary>
        /// <typeparam name="T">Type of the component</typeparam>
        private class ReusableComponentList<T> : IEnumerable<T> where T : class 
        {
            /// <summary>
            /// The item list
            /// </summary>
            private readonly Lazy<IList<T>> itemList;

            /// <summary>
            /// Initializes a new instance of the <see cref="ReusableComponentList{T}"/> class.
            /// </summary>
            /// <param name="items">The items.</param>
            public ReusableComponentList(IEnumerable<T> items)
            {
                this.itemList = new Lazy<IList<T>>(() => this.ProcessItems(items).ToList());
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
            /// </returns>
            public IEnumerator<T> GetEnumerator()
            {
                return this.itemList.Value.GetEnumerator();
            }

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
            /// </returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <summary>
            /// Processes the items.
            /// </summary>
            /// <param name="items">The items.</param>
            /// <returns>List of components and/or reusable components</returns>
            private IEnumerable<T> ProcessItems(IEnumerable<T> items)
            {
                var processedItems = new List<T>();

                foreach(var item in items.OfType<IReusableComponent>())
                {
                    var processedItem = ProcessItem(item);

                    if (processedItem != null && !(processedItem is IInvalidComponent))
                    {
                        processedItems.Add(processedItem);
                    }
                }

                return processedItems;
            }

            /// <summary>
            /// Processes the item for mapping.
            /// </summary>
            /// <param name="item">The item to be mapped.</param>
            /// <returns>The mapped item.</returns>
            private static T ProcessItem(IReusableComponent item)
            {
                T processedItem;

                if (item.ReusableComponent != null)
                {
                    var reusableComponent = item.ReusableComponent;

                    var reusableSection = reusableComponent as ISection;

                    if (reusableSection != null)
                    {
                        reusableSection.ShowInSummary = item.ShowInSummary;
                    }

                    processedItem = (T) reusableComponent;
                }
                else
                {
                    processedItem = (T) item;
                }

                return processedItem;
            }
        }
    }
}
