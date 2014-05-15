using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Context
{
    using Glass.Mapper.Sc;
    using Unic.Flex.DomainModel.Forms;

    public class FlexContext
    {
        private const string ContextKey = "FORM_CONTEXT";

        private Form form;

        public FlexContext()
        {
            this.SitecoreContext = new SitecoreContext();
        }

        public static FlexContext Current
        {
            get
            {
                return Sitecore.Context.Items[ContextKey] as FlexContext ?? new FlexContext();
            }
        }

        public ISitecoreContext SitecoreContext { get; set; }

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

        private void Save()
        {
            Sitecore.Context.Items[ContextKey] = this;
        }
    }
}
