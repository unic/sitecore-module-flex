namespace Unic.Flex.Core.Context
{
    using System.Web;
    using Glass.Mapper.Sc;
    using Sitecore.Data.Items;
    using Sitecore.Sites;
    using Unic.Flex.Core.Plugs;
    using Unic.Flex.Model;
    using Unic.Flex.Model.Forms;

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
        private IForm form;

        /// <summary>
        /// The item
        /// </summary>
        private IItemBase item;

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
        public FlexContext(IContextService contextService, IPlugsService plugService, ISitecoreContext sitecoreContext)
        {
            this.contextService = contextService;
            this.plugService = plugService;
            this.sitecoreContext = sitecoreContext;

            this.SiteContext = Sitecore.Context.Site;
            this.LanguageName = Sitecore.Context.Language.Name;
        }

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        public virtual IItemBase Item
        {
            get
            {
                if (this.item == null)
                {
                    this.SetContextItem(Sitecore.Context.Item);
                }

                return this.item;
            }

            set
            {
                this.item = value;
            }
        }

        /// <summary>
        /// Gets or sets the form.
        /// </summary>
        /// <value>
        /// The form.
        /// </value>
        public virtual IForm Form
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
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public virtual string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the site context.
        /// </summary>
        /// <value>
        /// The site context.
        /// </value>
        public virtual SiteContext SiteContext { get; set; }

        public virtual string LanguageName { get; set; }

        public virtual Language Language => Sitecore.Context.Language;

        /// <summary>
        /// Sets the context item.
        /// </summary>
        /// <param name="contextItem">The context item.</param>
        public virtual void SetContextItem(Item contextItem)
        {
            Sitecore.Context.Item = contextItem;
            this.Item = contextItem != null ? this.sitecoreContext.GetItem<ItemBase>(contextItem.ID.ToGuid(), inferType: true) : null;
        }
    }
}
