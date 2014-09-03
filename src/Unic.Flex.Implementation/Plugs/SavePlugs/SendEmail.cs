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
        private readonly IConfigurationManager configurationManager;

        public SendEmail(IConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;
        }
        
        public override void Execute(Form form)
        {
            Assert.ArgumentNotNull(form, "form");

            Log.Error("---------- " + this.configurationManager.Get<SendEmailPlugConfiguration>(c => c.From), this);
        }
    }
}
