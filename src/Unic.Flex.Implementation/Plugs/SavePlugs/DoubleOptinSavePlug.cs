﻿namespace Unic.Flex.Implementation.Plugs.SavePlugs
{
    using System.Collections.Generic;
    using Core.Mailing;
    using Core.Utilities;
    using Database;
    using Glass.Mapper.Sc;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Mailers;
    using Model;
    using Model.Forms;
    using Model.GlassExtensions.Attributes;
    using Model.Plugs;
    using Model.Specifications;
    using Sitecore.Diagnostics;
    using Sitecore.Links;
    using Constants = Definitions.Constants;

    [SitecoreType(TemplateId = "{F8239638-A672-491F-BBD4-A59CD3090C8B}")]
    public class DoubleOptinSavePlug : SavePlugBase
    {
        private readonly IMailRepository mailRepository;
        private readonly IDoubleOptinSavePlugMailer doubleOptinSavePlugMailer;
        private readonly ISaveToDatabaseService saveToDatabaseService;
        private readonly ISitecoreContext sitecoreContext;

        public DoubleOptinSavePlug(IMailRepository mailRepository, IDoubleOptinSavePlugMailer doubleOptinSavePlugMailer, ISaveToDatabaseService saveToDatabaseService, ISitecoreContext sitecoreContext)
        {
            this.mailRepository = mailRepository;
            this.doubleOptinSavePlugMailer = doubleOptinSavePlugMailer;
            this.saveToDatabaseService = saveToDatabaseService;
            this.sitecoreContext = sitecoreContext;
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

        [SitecoreChildren(IsLazy = true, InferType = true)]
        public virtual IEnumerable<ISavePlug> SavePlugs { get; set; }

        public override void Execute(IForm form)
        {
            Assert.ArgumentNotNull(form, "form");

            var recordId = this.saveToDatabaseService.Save(form);
            var doubleOptinLink = CreateConfirmationLink(form.Id, recordId);
            var mailMessage = this.doubleOptinSavePlugMailer.GetMessage(form, this, doubleOptinLink);
            
            this.mailRepository.SendMail(mailMessage);
        }

        public override bool IsAsync => false;

        private string CreateConfirmationLink(string formId, int optInRecordId)
        {
            var optInHash = this.CreateOptInHash(optInRecordId, this.To, formId);
            var url = sitecoreContext.GetCurrentItem<ItemBase>().Url;
            var link = $"{url}?{Constants.ScActionQueryKey}={Constants.OptionQueryKey}&{Constants.OptInFormIdKey}={formId}&{Constants.OptInRecordIdKey}={optInRecordId}&{Constants.OptInHashKey}={optInHash}";

            return link;
        }

        private string CreateOptInHash(int optInRecordId, string email, string formId)
        {
            var salt = Sitecore.Configuration.Settings.GetSetting("Flex.DoubleOptin.Salt");
            var stringToHash = $"{optInRecordId}|{email}|{formId}";
            return SecurityUtil.GenerateHash(stringToHash, salt);
        }
    }
}
