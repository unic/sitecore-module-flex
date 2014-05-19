using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Context
{
    using Glass.Mapper.Sc;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Sites;
    using Unic.Flex.Model;
    using Unic.Flex.Model.Forms;
    using Unic.Flex.Model.Steps;

    public class FlexContext
    {
        private const string ContextKey = "FORM_CONTEXT";

        private Form form;

        private string previousStepUrl;
        
        private string nextStepUrl;

        public FlexContext()
        {
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
                return this.form;
            }

            set
            {
                this.form = value;
                this.Save();
            }
        }

        public string PreviousStepUrl
        {
            get
            {
                if (this.previousStepUrl == null)
                {
                    var linkedSteps = new LinkedList<StepBase>(this.Form.Steps);
                    var currentStep = linkedSteps.Find(this.Form.GetActiveStep());
                    if (currentStep == null) throw new Exception("Could not convert steps to linked list");

                    var previousStep = currentStep.Previous;
                    if (previousStep == null)
                    {
                        this.PreviousStepUrl = string.Empty;
                    }
                    else if (previousStep.Equals(linkedSteps.First))
                    {
                        this.PreviousStepUrl = this.Item.Url;
                    }
                    else
                    {
                        this.PreviousStepUrl = previousStep.Value.GetUrl();
                    }
                }

                return this.previousStepUrl;
            }
            set
            {
                this.previousStepUrl = value;
                this.Save();
            }
        }

        public string NextStepUrl
        {
            get
            {
                if (this.nextStepUrl == null)
                {
                    var linkedSteps = new LinkedList<StepBase>(this.Form.Steps);
                    var currentStep = linkedSteps.Find(this.Form.GetActiveStep());
                    if (currentStep == null) throw new Exception("Could not convert steps to linked list");

                    var nextStep = currentStep.Next;
                    this.NextStepUrl = nextStep != null ? nextStep.Value.GetUrl() : string.Empty;
                }
                
                return this.nextStepUrl;
            }
            set
            {
                this.nextStepUrl = value;
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
