namespace Unic.Flex.Implementation.Plugs.SavePlugs
{
    using System.Collections.Generic;
    using Core.Context;
    using Core.Mailing;
    using Database;
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Mailers;
    using Model.Fields;
    using Model.Forms;
    using Model.Plugs;
    using Services;
    using Sitecore.Diagnostics;

    [SitecoreType(TemplateId = "{F8239638-A672-491F-BBD4-A59CD3090C8B}")]
    public class DoubleOptinSavePlug : SavePlugBase
    {
        private readonly IMailRepository mailRepository;
        private readonly IDoubleOptinSavePlugMailer doubleOptinSavePlugMailer;
        private readonly ISaveToDatabaseService saveToDatabaseService;
        private readonly IDoubleOptinService doubleOptinService;
        private readonly IFlexContext flexContext;
        private readonly IDoubleOptinLinkService doubleOptinLinkService;

        public DoubleOptinSavePlug(IMailRepository mailRepository, IDoubleOptinSavePlugMailer doubleOptinSavePlugMailer, ISaveToDatabaseService saveToDatabaseService, IDoubleOptinService doubleOptinService, IFlexContext flexContext, IDoubleOptinLinkService doubleOptinLinkService)
        {
            this.mailRepository = mailRepository;
            this.doubleOptinSavePlugMailer = doubleOptinSavePlugMailer;
            this.saveToDatabaseService = saveToDatabaseService;
            this.doubleOptinService = doubleOptinService;
            this.flexContext = flexContext;
            this.doubleOptinLinkService = doubleOptinLinkService;
        }

        [SitecoreField("From")]
        public virtual string From { get; set; }

        [SitecoreField("Reply To")]
        public virtual string ReplyTo { get; set; }

        [SitecoreField("To", Setting = SitecoreFieldSettings.InferType)]
        public virtual IField To { get; set; }

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

        [SitecoreField("Confirm Message")]
        public virtual string ConfirmMessage { get; set; }

        [SitecoreField("Redirect Url")]
        public virtual string RedirectUrl { get; set; }

        [SitecoreChildren(IsLazy = true, InferType = true)]
        public virtual IEnumerable<ISavePlug> SavePlugs { get; set; }

        public override void Execute(IForm form)
        {
            Assert.ArgumentNotNull(form, "form");

            var recordId = this.saveToDatabaseService.Save(form);

            if (Sitecore.Context.User.IsAuthenticated && (Sitecore.Context.User.Profile.Email.Equals(form.GetFieldValue(this.To), System.StringComparison.CurrentCultureIgnoreCase)))
            {
                this.doubleOptinService.ExecuteSubSavePlugs(this, flexContext, recordId.ToString());
                return;
            }

            var doubleOptinLink = this.doubleOptinLinkService.CreateConfirmationLink(form.Id, form.GetFieldValue(this.To), recordId.ToString());
            var mailMessage = this.doubleOptinSavePlugMailer.GetMessage(form, this, doubleOptinLink);

            this.mailRepository.SendMail(mailMessage);
        }

        public override bool IsAsync => false;
    }
}
