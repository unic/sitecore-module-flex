namespace Unic.Flex.Context
{
    using Sitecore.Data.Items;
    using Sitecore.Sites;
    using Unic.Flex.Model.DomainModel;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.ViewModel.Forms;

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
        ItemBase Item { get; set; }

        /// <summary>
        /// Gets or sets the form.
        /// </summary>
        /// <value>
        /// The form.
        /// </value>
        Form Form { get; set; }

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        IFormViewModel ViewModel { get; set; }

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

        /// <summary>
        /// Sets the context item.
        /// </summary>
        /// <param name="contextItem">The context item.</param>
        void SetContextItem(Item contextItem);
    }
}
