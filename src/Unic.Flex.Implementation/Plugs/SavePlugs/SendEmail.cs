namespace Unic.Flex.Implementation.Plugs.SavePlugs
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Sitecore.Diagnostics;
    using Unic.Configuration;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.Implementation.Configuration;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Plugs.SavePlugs;

    [SitecoreType(TemplateId = "{DF5C2C2F-4A48-4206-9DFB-0DCDE27E2233}")]
    public class SendEmail : SavePlugBase
    {
        public IConfigurationManager ConfigurationManager { private get; set; }
        
        public override void Execute(Form form)
        {
            Assert.ArgumentNotNull(form, "form");

            this.ConfigurationManager = Container.Resolve<IConfigurationManager>();

            Log.Error("---------- " + this.ConfigurationManager.Get<SendEmailPlugConfiguration>(c => c.From), this);
        }
    }
}
