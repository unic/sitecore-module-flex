namespace Unic.Flex.Context
{
    using Glass.Mapper.Sc;
    using Sitecore.Data.Items;
    using System.Web;
    using Unic.Flex.Model.DomainModel;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.ViewModel.Forms;
    using Unic.Flex.Plugs;

    public class FlexContext : IFlexContext
    {
        private readonly IContextService contextService;

        private readonly IPlugsService plugService;

        private readonly ISitecoreContext sitecoreContext;

        private Form form;

        private bool hasValuesPopulated;

        public FlexContext(IContextService contextService, IPlugsService plugService, ISitecoreContext sitecoreContext)
        {
            this.contextService = contextService;
            this.plugService = plugService;
            this.sitecoreContext = sitecoreContext;

            this.SetContextItem(Sitecore.Context.Item);
        }

        public ItemBase Item { get; set; }

        public Form Form
        {
            get
            {
                if (!this.hasValuesPopulated && HttpContext.Current.Session != null)
                {
                    this.contextService.PopulateFormValues(this.form);
                    this.plugService.ExecuteLoadPlugs(this.form);
                    this.hasValuesPopulated = true;
                }
                
                return this.form;
            }

            set
            {
                this.form = value;
            }
        }

        public IFormViewModel ViewModel { get; set; }

        public void SetContextItem(Item contextItem)
        {
            Sitecore.Context.Item = contextItem;
            this.Item = contextItem != null ? this.sitecoreContext.GetItem<ItemBase>(contextItem.ID.ToGuid(), inferType: true) : null;
        }
    }
}
