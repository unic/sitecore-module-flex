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
            if (ControllerContext == null)
            {
                Initialize(HttpContext.Current.Request.RequestContext);
            }
            
            theme = plug.Theme != null ? plug.Theme.Value : string.Empty;
            
            ViewBag.HtmlLayout = presentationService.ResolveView(ControllerContext, "Mailers/_Layout", theme);
            ViewBag.TextLayout = presentationService.ResolveView(ControllerContext, "Mailers/_Layout.text", theme);

            ViewBag.Form = form;
            ViewBag.Theme = theme;

            var fields = form.GetFields().ToList();
            ViewBag.Subject = mailService.ReplaceTokens(plug.Subject, fields);
            ViewBag.HtmlMailIntroduction = mailService.ReplaceTokens(plug.HtmlMailIntroduction, fields);
            ViewBag.HtmlMailFooter = mailService.ReplaceTokens(plug.HtmlMailFooter, fields);
            ViewBag.TextMailIntroduction = mailService.ReplaceTokens(plug.TextMailIntroduction, fields);
            ViewBag.TextMailFooter = mailService.ReplaceTokens(plug.TextMailFooter, fields);

            var useGlobalConfig = IsGlobalConfigEnabled();
            var from = this.mailHelper.GetEmailAddresses(configurationManager.Get<SendEmailPlugConfiguration>(c => c.From), plug.From, useGlobalConfig);
            var cc = this.mailHelper.GetEmailAddresses(configurationManager.Get<SendEmailPlugConfiguration>(c => c.Cc), plug.Cc, useGlobalConfig);
            var bcc = this.mailHelper.GetEmailAddresses(configurationManager.Get<SendEmailPlugConfiguration>(c => c.Bcc), plug.Bcc, useGlobalConfig);
            
            var to = useGlobalConfig ? string.Empty : GetMappedEmailFromFields(plug.ReceiverFields, form, plug);
            if (string.IsNullOrWhiteSpace(to))
            {
                to = this.mailHelper.GetEmailAddresses(configurationManager.Get<SendEmailPlugConfiguration>(c => c.To), plug.To, useGlobalConfig);
            }

            var replyTo = useGlobalConfig ? string.Empty : this.mailHelper.GetEmailFromFields(plug.ReplyToFields, form);
            if (string.IsNullOrWhiteSpace(replyTo))
            {
                replyTo = this.mailHelper.GetEmailAddresses(configurationManager.Get<SendEmailPlugConfiguration>(c => c.ReplyTo), plug.ReplyTo, useGlobalConfig);
            }
            
            return Populate(x =>
            {
                x.ViewName = presentationService.ResolveView(ControllerContext, "Mailers/SavePlug/Form", theme);
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
            return presentationService.ResolveView(ControllerContext, "Mailers/SavePlug/Form.text", theme);
        }

        protected virtual bool IsGlobalConfigEnabled()
        {
            return Settings.GetBoolSetting("Flex.EmailAddresses.AlwaysUseGlobalConfig", false);
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
