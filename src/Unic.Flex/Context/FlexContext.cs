namespace Unic.Flex.Context
{
    using Glass.Mapper.Sc;
    using Ninject;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Sites;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.Model.DomainModel;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Steps;
    using Unic.Flex.Plugs;

    internal class FlexContext
    {
        private const string ContextKey = "FORM_CONTEXT";

        [Inject]
        public IContextService ContextService { private get; set; }

        [Inject]
        public IPlugsService PlugService { private get; set; }

        private Form form;

        private bool hasValuesPopulated;

        private FlexContext()
        {
            Container.Kernel.Inject(this);
            
            this.Database = Sitecore.Context.Database;
            this.SiteContext = Sitecore.Context.Site;
            this.Device = Sitecore.Context.Device;
            this.SitecoreContext = new SitecoreContext();

            this.SetContextItem(Sitecore.Context.Item);
        }

        public static FlexContext Current
        {
            get
            {
                return Sitecore.Context.Items[ContextKey] as FlexContext ?? new FlexContext();
            }
        }

        public ISitecoreContext SitecoreContext { get; set; }

        public ItemBase Item { get; set; }

        public SiteContext SiteContext { get; set; }

        public Database Database { get; set; }

        public DeviceItem Device { get; set; }

        public Form Form
        {
            get
            {
                if (!this.hasValuesPopulated && HttpContext.Current.Session != null)
                {
                    this.ContextService.PopulateFormValues(this.form);
                    this.PlugService.ExecuteLoadPlugs(this.form);
                    this.hasValuesPopulated = true;
                }
                
                return this.form;
            }

            set
            {
                this.form = value;
                this.Save();
            }
        }

        public void SetContextItem(Item contextItem)
        {
            Sitecore.Context.Item = contextItem;
            this.Item = contextItem != null ? this.SitecoreContext.GetItem<ItemBase>(contextItem.ID.ToGuid(), inferType: true) : null;
            this.Save();
        }

        private void Save()
        {
            Sitecore.Context.Items[ContextKey] = this;
        }
    }
}
