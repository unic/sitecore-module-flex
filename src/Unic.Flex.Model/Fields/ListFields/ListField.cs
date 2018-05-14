namespace Unic.Flex.Model.Fields.ListFields
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.Components;
    using Unic.Flex.Model.DataProviders;
    using Unic.Flex.Model.GlassExtensions.Attributes;
    using Unic.Flex.Model.SortOrder;
    using Unic.Flex.Model.Specifications;

    /// <summary>
    /// Base class for all list fields.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <typeparam name="TType">The type of the data items.</typeparam>
    public abstract class ListField<TValue, TType> : FieldBase<TValue>, ICascadingDependency where TType : IDataItem
    {
        /// <summary>
        /// The items
        /// </summary>
        private IList<TType> items;

        /// <summary>
        /// The data provider
        /// </summary>
        private IDataProvider<TType> dataProvider;

        /// <summary>
        /// Private field for storing the is hidden property.
        /// </summary>
        private bool? isHidden;
        
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        [SitecoreIgnore]
        public virtual IList<TType> Items
        {
            get
            {
                // lazy loading, but only for non cascading fields
                if (!this.IsCascadingField && this.items != null) return this.items;
                
                // check for valid provider
                if (this.DataProvider == null)
                {
                    this.items = new List<TType>();
                    return this.items;
                }
                
                // get items
                var providerItems = this.DataProvider.GetItems();

                // sort items
                if (this.ItemsSortOrder != null && !string.IsNullOrWhiteSpace(this.ItemsSortOrder.Value))
                {
                    var strategy = StrategyFactory.CreateInstance(this.ItemsSortOrder.Value);
                    if (strategy != null) providerItems = strategy.Sort(providerItems);
                }

                // return
                this.items = providerItems.ToList();
                return this.items;
            }
        }

        /// <summary>
        /// Gets or sets the items provider.
        /// </summary>
        /// <value>
        /// The items provider.
        /// </value>
        [SitecoreSharedField("Data Provider", Setting = SitecoreFieldSettings.InferType)]
        public virtual IDataProvider<TType> DataProvider
        {
            get
            {
                // lazy loading, but only for non cascading fields
                if (!this.IsCascadingField && this.dataProvider != null) return this.dataProvider;

                // get data provider from dependent field
                if (this.IsCascadingField && this.DependentField != null && this.DependentField.Value != null)
                {
                    // cascading fields are only possible with ListItem's
                    var dependentListField = this.DependentField as ListField<TValue, ListItem>;
                    if (dependentListField != null)
                    {
                        var dependentDataProvider = dependentListField.DataProvider;
                        if (dependentDataProvider != null)
                        {
                            var dependentItems = dependentDataProvider.GetItems();
                            var dependentValue = dependentItems.FirstOrDefault(item => item.Value != null && item.Value.Equals(this.DependentField.Value.ToString()));
                            if (dependentValue != null)
                            {
                                this.dataProvider = dependentValue.CascadingDataProvider as IDataProvider<TType>;
                            }
                        }
                    }
                }

                return this.dataProvider;
            }

            set
            {
                this.dataProvider = value;
            }
        }

        /// <summary>
        /// Gets or sets the items sort order.
        /// </summary>
        /// <value>
        /// The items sort order.
        /// </value>
        [SitecoreSharedField("Items Sort Order", Setting = SitecoreFieldSettings.InferType)]
        public virtual Specification ItemsSortOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this field is a cascading field.
        /// </summary>
        /// <value>
        /// <c>true</c> if this field is a cascading field; otherwise, <c>false</c>.
        /// </value>
        [SitecoreField("Is Cascading Field")]
        public virtual bool IsCascadingField { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is hidden.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is hidden; otherwise, <c>false</c>.
        /// </value>
        [SitecoreIgnore]
        public override bool IsHidden
        {
            get
            {
                // lazy loading, but only for non cascading fields
                if (!this.IsCascadingField && this.isHidden.HasValue) return this.isHidden.Value;

                if (this.IsCascadingField && this.DependentField != null)
                {
                    // value of dependent field is empty, so this field is hidden
                    var dependendValueString = this.DependentField.Value as string;
                    if (dependendValueString == null)
                    {
                        this.isHidden = true;
                        return this.isHidden.Value;
                    }

                    // cascading fields are only possible with ListItems
                    var currentItems = this.Items as IList<ListItem>;
                    if (currentItems != null)
                    {
                        // check if current value is empty -> then it's not hidden when we have any not-empty values in the list
                        var stringValue = this.Value as string;
                        if (string.IsNullOrWhiteSpace(stringValue))
                        {
                            this.isHidden = currentItems.All(item => string.IsNullOrWhiteSpace(item.Value));
                            return this.isHidden.Value;
                        }
                        
                        // otherwise check if our current selected value is within the current item collection
                        this.isHidden = currentItems.All(item => item.Value != stringValue);
                        return this.isHidden.Value;
                    }
                }
                
                this.isHidden = !this.Items.Any() || (!this.IsCascadingField && base.IsHidden);
                return this.isHidden.Value;
            }
        }

        /// <summary>
        /// Gets the text value.
        /// </summary>
        /// <value>
        /// The text value.
        /// </value>
        [SitecoreIgnore]
        public override string TextValue
        {
            get
            {
                var value = this.DataProvider != null
                                ? this.DataProvider.GetTextValue<TValue>(this.Value)
                                : string.Empty;
                
                return !string.IsNullOrWhiteSpace(value) ? value : Definitions.Constants.EmptyFlexFieldDefaultValue;
            }
        }

        /// <summary>
        /// Binds the needed attributes and properties after converting from domain model to the view model
        /// </summary>
        public override void BindProperties()
        {
            if (this.DependentField != null && this.IsCascadingField)
            {
                this.ContainerAttributes.Add("data-flexform-dependent", "{" + HttpUtility.HtmlEncode(string.Format("\"from\": \"{0}\", \"url\": \"{1}\"", this.DependentField.Id, this.GetCascadingDataUrl())) + "}");
            }

            base.BindProperties();
        }

        /// <summary>
        /// Gets the cascading data URL.
        /// </summary>
        /// <returns>Url to the cascading data action</returns>
        private string GetCascadingDataUrl()
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            return urlHelper.RouteUrl(Constants.MvcRouteName, new { controller = "Flex", action = "CascadingField", field = this.ItemId, sc_lang = Sitecore.Context.Language });
        }
    }
}
