namespace Unic.Flex.Implementation.Plugs.SavePlugs
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Sitecore.Diagnostics;
    using Unic.Configuration;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.Implementation.Configuration;
    using Unic.Flex.Implementation.Mailers;
    using Unic.Flex.Mailing;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Global;
    using Unic.Flex.Model.DomainModel.Plugs.SavePlugs;
    using Unic.Flex.Model.GlassExtensions.Attributes;

    [SitecoreType(TemplateId = "{DF5C2C2F-4A48-4206-9DFB-0DCDE27E2233}")]
    public class SendEmail : SavePlugBase
    {
        private readonly IMailRepository mailRepository;

        private readonly ISavePlugMailer savePlugMailer;

        [SitecoreSharedField("Theme")]
        public virtual Specification Theme { get; set; }

        public SendEmail(IMailRepository mailRepository, ISavePlugMailer savePlugMailer)
        {
            this.mailRepository = mailRepository;
            this.savePlugMailer = savePlugMailer;
        }
        
        public override void Execute(Form form)
        {
            Assert.ArgumentNotNull(form, "form");

            var mailMessage = this.savePlugMailer.GetMessage(form, this.Theme != null ? this.Theme.Value : string.Empty);
            this.mailRepository.SendMail(mailMessage);
        }
    }
}
