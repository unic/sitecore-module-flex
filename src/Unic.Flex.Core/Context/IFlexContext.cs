namespace Unic.Flex.Core.Context
{
    using Sitecore.Data.Items;
    using Sitecore.Globalization;
    using Sitecore.Sites;
    using Unic.Flex.Model;
    using Unic.Flex.Model.Forms;

    /// <summary>
    /// The Flex context stores different type of properties which needs to be available during the requests.
    /// </summary>
    public interface IFlexContext
    {
        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        IItemBase Item { get; set; }

        /// <summary>
        /// Gets or sets the form.
        /// </summary>
        /// <value>
        /// The form.
        /// </value>
        IForm Form { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the site context.
        /// </summary>
        /// <value>
        /// The site context.
        /// </value>
        SiteContext SiteContext { get; set; }

        string LanguageName { get; set; }

        Language Language { get; }

        /// <summary>
        /// Sets the context item.
        /// </summary>
        /// <param name="contextItem">The context item.</param>
        void SetContextItem(Item contextItem);
    }
}
