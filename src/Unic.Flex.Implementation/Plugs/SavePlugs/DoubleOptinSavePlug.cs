namespace Unic.Flex.Implementation.Plugs.SavePlugs
{
    using Core.Mailing;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Mailers;
    using Model.Forms;
    using Model.GlassExtensions.Attributes;
    using Model.Plugs;
    using Model.Specifications;
    using Sitecore.Diagnostics;

    [SitecoreType(TemplateId = "{F8239638-A672-491F-BBD4-A59CD3090C8B}")]
    public class DoubleOptinSavePlug : SavePlugBase
    {
        private readonly IMailRepository mailRepository;
        private readonly IDoubleOptinSavePlugMailer doubleOptinSavePlugMailer;

        public DoubleOptinSavePlug(IMailRepository mailRepository, IDoubleOptinSavePlugMailer doubleOptinSavePlugMailer)
        {
          this.mailRepository = mailRepository;
            this.doubleOptinSavePlugMailer = doubleOptinSavePlugMailer;
        }

        [SitecoreSharedField("Theme")]
        public virtual Specification Theme { get; set; }

        [SitecoreField("From")]
        public virtual string From { get; set; }

        [SitecoreField("Reply To")]
        public virtual string ReplyTo { get; set; }

        [SitecoreField("To")]
        public virtual string To { get; set; }

        [SitecoreField("Cc")]
        public virtual string Cc { get; set; }

        [SitecoreField("Bcc")]
        public virtual string Bcc { get; set; }

        [SitecoreField("Subject")]
        public virtual string Subject { get; set; }

        [SitecoreField("Html Mail")]
        public virtual string HtmlMail { get; set; }

        [SitecoreField("Text Mail")]
        public virtual string TextMail { get; set; }

        [SitecoreField("Double Optin Link")]
        public virtual string DoubleOptinLink { get; set; }

        public override void Execute(IForm form)
        {
            Assert.ArgumentNotNull(form, "form");

            var mailMessage = this.doubleOptinSavePlugMailer.GetMessage(form, this);
            this.mailRepository.SendMail(mailMessage);
        }

        public override bool IsAsync => false;
    }
}
