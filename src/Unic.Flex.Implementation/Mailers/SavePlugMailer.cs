namespace Unic.Flex.Implementation.Mailers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using System.Web;
    using Configuration;
    using Core.Mailing;
    using Core.Presentation;
    using Fields.InputFields;
    using Glass.Mapper;
    using Model.Fields;
    using Model.Forms;
    using Mvc.Mailer;
    using Plugs.SavePlugs;
    using Unic.Configuration.Core;
    using Validators;
    using Settings = Sitecore.Configuration.Settings;

    public class SavePlugMailer : MailerBase, ISavePlugMailer
    {
        private readonly IPresentationService presentationService;
        private readonly IConfigurationManager configurationManager;
        private readonly IMailService mailService;
        private readonly IMailerHelper mailHelper;
        private string theme;

        public SavePlugMailer(IPresentationService presentationService, IConfigurationManager configurationManager, IMailService mailService, IMailerHelper mailHelper)
        {
            this.presentationService = presentationService;
            this.configurationManager = configurationManager;
            this.mailService = mailService;
            this.mailHelper = mailHelper;
        }

        public virtual MvcMailMessage GetMessage(IForm form, SendEmail plug)
        {
            if (this.ControllerContext == null)
            {
                Initialize(HttpContext.Current.Request.RequestContext);
            }
            
            this.theme = plug.Theme != null ? plug.Theme.Value : string.Empty;
            
            ViewBag.HtmlLayout = this.presentationService.ResolveView(this.ControllerContext, "Mailers/_Layout", this.theme);
            ViewBag.TextLayout = this.presentationService.ResolveView(this.ControllerContext, "Mailers/_Layout.text", this.theme);

            ViewBag.Form = form;
            ViewBag.Theme = this.theme;

            var fields = form.GetFields().ToList();
            ViewBag.Subject = this.mailService.ReplaceTokens(plug.Subject, fields);
            ViewBag.HtmlMailIntroduction = this.mailService.ReplaceTokens(plug.HtmlMailIntroduction, fields);
            ViewBag.HtmlMailFooter = this.mailService.ReplaceTokens(plug.HtmlMailFooter, fields);
            ViewBag.TextMailIntroduction = this.mailService.ReplaceTokens(plug.TextMailIntroduction, fields);
            ViewBag.TextMailFooter = this.mailService.ReplaceTokens(plug.TextMailFooter, fields);

            var useGlobalConfig = IsGlobalConfigEnabled();
            var from = this.mailHelper.GetEmailAddresses(this.configurationManager.Get<SendEmailPlugConfiguration>(c => c.From), plug.From, useGlobalConfig);
            var cc = this.mailHelper.GetEmailAddresses(this.configurationManager.Get<SendEmailPlugConfiguration>(c => c.Cc), plug.Cc, useGlobalConfig);
            var bcc = this.mailHelper.GetEmailAddresses(this.configurationManager.Get<SendEmailPlugConfiguration>(c => c.Bcc), plug.Bcc, useGlobalConfig);
            
            var to = useGlobalConfig ? string.Empty : GetMappedEmailFromFields(plug.ReceiverFields, form, plug);
            if (string.IsNullOrWhiteSpace(to))
            {
                to = this.mailHelper.GetEmailAddresses(this.configurationManager.Get<SendEmailPlugConfiguration>(c => c.To), plug.To, useGlobalConfig);
            }

            var replyTo = useGlobalConfig ? string.Empty : this.mailHelper.GetEmailFromFields(plug.ReplyToFields, form);
            if (string.IsNullOrWhiteSpace(replyTo))
            {
                replyTo = this.mailHelper.GetEmailAddresses(this.configurationManager.Get<SendEmailPlugConfiguration>(c => c.ReplyTo), plug.ReplyTo, useGlobalConfig);
            }
            
            return this.Populate(x =>
            {
                x.ViewName = this.presentationService.ResolveView(this.ControllerContext, "Mailers/SavePlug/Form", this.theme);
                x.Subject = ViewBag.Subject;

                x.From = new MailAddress(from);
                to.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ForEach(address => x.To.Add(address));
                if (!string.IsNullOrWhiteSpace(cc)) cc.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ForEach(address => x.CC.Add(address));
                if (!string.IsNullOrWhiteSpace(bcc)) bcc.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ForEach(address => x.Bcc.Add(address));
                if (!string.IsNullOrWhiteSpace(replyTo)) x.ReplyToList.Add(replyTo);

                if (plug.SendAttachments)
                {
                    foreach (var fileField in fields.OfType<FileUploadField>().Where(field => field.Value != null))
                    {
                        x.Attachments.Add(new Attachment(new MemoryStream(fileField.Value.Data), fileField.Value.FileName));
                    }
                }
            });
        }

        public override string TextViewName(string viewName)
        {
            return this.presentationService.ResolveView(this.ControllerContext, "Mailers/SavePlug/Form.text", this.theme);
        }

        protected virtual bool IsGlobalConfigEnabled()
        {
            return Settings.GetBoolSetting(Definitions.Constants.AlwaysUseGlobalConfig, false);
        }

        private static string GetMappedEmailFromFields(IEnumerable<IField> fields, IForm form, SendEmail plug)
        {
            foreach (var field in fields)
            {
                var email = GetMappedEmailFromField(field, form, plug);
                if (!string.IsNullOrWhiteSpace(email)) return email;
            }

            return string.Empty;
        }

        private static string GetMappedEmailFromField(IField field, IForm form, SendEmail plug)
        {
            var fieldValue = form.GetFieldValue(field);
            if (string.IsNullOrWhiteSpace(fieldValue)) return string.Empty;

            var emailValidator = new EmailValidator();
            var email = plug.ReceiverEmailMapping.Get(fieldValue);
            if (!string.IsNullOrWhiteSpace(email) && email.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).All(emailValidator.IsValid))
            {
                return email;
            }

            return emailValidator.IsValid(fieldValue) ? fieldValue : string.Empty;
        }
    }
}
