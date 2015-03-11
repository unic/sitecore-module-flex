namespace Unic.Flex.Implementation.Fields.InputFields
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Core.Globalization;
    using Unic.Flex.Model.DataProviders;
    using Unic.Flex.Model.Fields.InputFields;
    using Unic.Flex.Model.GlassExtensions.Attributes;

    /// <summary>
    /// Field for adding autocompletion.
    /// </summary>
    [SitecoreType(TemplateId = "{A9FD64D4-F178-498F-9BD1-D6264FD4B452}")]
    public class AutoCompleteField : InputField<string>
    {
        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        [SitecoreField("Default Value")]
        public override string DefaultValue { get; set; }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public virtual IList<string> Items
        {
            get
            {
                return this.DataProvider != null ? this.DataProvider.GetItems().Select(item => item.Text).ToList() : new List<string>();
            }
        }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public override string ViewName
        {
            get
            {
                return "Fields/InputFields/AutoComplete";
            }
        }

        /// <summary>
        /// Gets or sets the items provider.
        /// </summary>
        /// <value>
        /// The items provider.
        /// </value>
        [SitecoreSharedField("Data Provider", Setting = SitecoreFieldSettings.InferType)]
        public virtual IDataProvider<ListItem> DataProvider { get; set; }

        /// <summary>
        /// Binds the properties.
        /// </summary>
        public override void BindProperties()
        {
            var translationService = Core.DependencyInjection.DependencyResolver.Resolve<IDictionaryRepository>();

            base.BindProperties();

            this.Attributes.Add("aria-multiline", false);
            this.Attributes.Add("role", "textbox");
            this.Attributes.Add("data-init", "flexautocomplete");
            this.Attributes.Add("data-flexautocomplete-options", "{\"noResults\": \"" + translationService.GetText("No Results") + "\", \"sendAll\": false, \"url\": \"" + this.GetProviderUrl() + "\"}");

            this.AddCssClass("flex_singletextfield");
        }

        /// <summary>
        /// Gets the provider URL.
        /// </summary>
        /// <returns>Url for Ajax request to retrieve data.</returns>
        private string GetProviderUrl()
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            return urlHelper.Action("AutoCompleteField", "Flex", new { field = this.ItemId, sc_lang = Sitecore.Context.Language });
        }
    }
}
