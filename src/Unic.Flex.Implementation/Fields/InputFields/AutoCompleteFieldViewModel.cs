namespace Unic.Flex.Implementation.Fields.InputFields
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Unic.Flex.Core.DependencyInjection;
    using Unic.Flex.Core.Globalization;
    using Unic.Flex.Model.ViewModel.Fields.InputFields;

    /// <summary>
    /// View model for the field with autocompletion.
    /// </summary>
    public class AutoCompleteFieldViewModel : InputFieldViewModel<string>
    {
        /// <summary>
        /// Gets or sets the item identifier.
        /// </summary>
        /// <value>
        /// The item identifier.
        /// </value>
        public virtual Guid ItemId { get; set; }
        
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
        /// Binds the properties.
        /// </summary>
        public override void BindProperties()
        {
            var translationService = Container.Resolve<IDictionaryRepository>();
            
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
