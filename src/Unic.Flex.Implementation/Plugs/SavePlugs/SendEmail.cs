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

        [SitecoreField("From")]
        public virtual string From { get; set; }

        [SitecoreField("To")]
        public virtual string To { get; set; }

        [SitecoreField("Cc")]
        public virtual string Cc { get; set; }

        [SitecoreField("Bcc")]
        public virtual string Bcc { get; set; }

        [SitecoreField("Subject")]
        public virtual string Subject { get; set; }

        [SitecoreField("Html Mail Introduction")]
        public virtual string HtmlMailIntroduction { get; set; }

        [SitecoreField("Html Mail Footer")]
        public virtual string HtmlMailFooter { get; set; }

        [SitecoreField("Text Mail Introduction")]
        public virtual string TextMailIntroduction { get; set; }

        [SitecoreField("Text Mail Footer")]
        public virtual string TextMailFooter { get; set; }

        public SendEmail(IMailRepository mailRepository, ISavePlugMailer savePlugMailer)
        {
            this.mailRepository = mailRepository;
            this.savePlugMailer = savePlugMailer;
        }
        
        public override void Execute(Form form)
        {
            Assert.ArgumentNotNull(form, "form");

            var mailMessage = this.savePlugMailer.GetMessage(form, this);
            this.mailRepository.SendMail(mailMessage);
        }
    }
}
