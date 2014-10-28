namespace Unic.Flex.Context
{
    using Glass.Mapper.Sc;
    using Ninject;
    using Sitecore.Data.Items;
    using System.Web;
    using Sitecore.Sites;
    using Unic.Flex.Definitions;
    using Unic.Flex.Model.DomainModel;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.ViewModel.Forms;
    using Unic.Flex.Plugs;

    /// <summary>
    /// The Flex context stores different type of properties which needs to be available during the requests. This class is
    /// injected in request scope.
    /// </summary>
    public class FlexContext : IFlexContext
    {
        /// <summary>
        /// The context service
        /// </summary>
        private readonly IContextService contextService;

        /// <summary>
        /// The plug service
        /// </summary>
        private readonly IPlugsService plugService;

        /// <summary>
        /// The sitecore context
        /// </summary>
        private readonly ISitecoreContext sitecoreContext;

        /// <summary>
        /// The form
        /// </summary>
        private Form form;

        /// <summary>
        /// Field to store if the form values has already be populated from the user session.
        /// </summary>
        private bool hasValuesPopulated;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlexContext"/> class.
        /// </summary>
        /// <param name="contextService">The context service.</param>
        /// <param name="plugService">The plug service.</param>
        /// <param name="sitecoreContext">The sitecore context.</param>
        public FlexContext(IContextService contextService, IPlugsService plugService, [Named(Constants.InjectionName)]ISitecoreContext sitecoreContext)
        {
            this.contextService = contextService;
            this.plugService = plugService;
            this.sitecoreContext = sitecoreContext;

            this.SetContextItem(Sitecore.Context.Item);
            this.SiteContext = Sitecore.Context.Site;
        }

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        public ItemBase Item { get; set; }

        /// <summary>
        /// Gets or sets the form.
        /// </summary>
        /// <value>
        /// The form.
        /// </value>
        public Form Form
        {
            get
            {
                if (this.form != null && !this.hasValuesPopulated && HttpContext.Current.Session != null)
                {
                    this.hasValuesPopulated = true;
                    this.contextService.PopulateFormValues(this.form);
                    this.plugService.ExecuteLoadPlugs(this);
                }
                
                return this.form;
            }

            set
            {
                this.form = value;
            }
        }

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public IFormViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the site context.
        /// </summary>
        /// <value>
        /// The site context.
        /// </value>
        public SiteContext SiteContext { get; set; }

        /// <summary>
        /// Sets the context item.
        /// </summary>
        /// <param name="contextItem">The context item.</param>
        public void SetContextItem(Item contextItem)
        {
            Sitecore.Context.Item = contextItem;
            this.Item = contextItem != null ? this.sitecoreContext.GetItem<ItemBase>(contextItem.ID.ToGuid(), inferType: true) : null;
        }
    }
}
