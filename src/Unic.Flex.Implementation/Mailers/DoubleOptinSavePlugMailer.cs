namespace Unic.Flex.Implementation.Mailers
{
    using System;
    using System.Linq;
    using System.Net.Mail;
    using System.Web;
    using Configuration;
    using Core.Mailing;
    using Core.Presentation;
    using Glass.Mapper;
    using Model.Forms;
    using Mvc.Mailer;
    using Plugs.SavePlugs;
    using Unic.Configuration.Core;
    using Settings = Sitecore.Configuration.Settings;

    public class DoubleOptinSavePlugMailer : MailerBase, IDoubleOptinSavePlugMailer
    {
        private readonly IPresentationService presentationService;
        private readonly IConfigurationManager configurationManager;
        private readonly IMailService mailService;
        private readonly IMailerHelper mailHelper;

        public DoubleOptinSavePlugMailer(IPresentationService presentationService, IConfigurationManager configurationManager, IMailService mailService, IMailerHelper mailHelper)
        {
            this.presentationService = presentationService;
            this.configurationManager = configurationManager;
            this.mailService = mailService;
            this.mailHelper = mailHelper;
        }

        public MvcMailMessage GetMessage(IForm form, DoubleOptinSavePlug plug, string doubleOptinLink)
        {
            // ensure the mailer has been initialized
            if (this.ControllerContext == null)
            {
                Initialize(HttpContext.Current.Request.RequestContext);
            }

            // get the layouts
            ViewBag.HtmlLayout = this.presentationService.ResolveView(this.ControllerContext, "Mailers/_Layout");
            ViewBag.TextLayout = this.presentationService.ResolveView(this.ControllerContext, "Mailers/_Layout.text");

            // add data
            ViewBag.Form = form;

            // add content
            var fields = form.GetFields().ToList();
            ViewBag.Subject = this.mailService.ReplaceTokens(plug.Subject, fields);
            ViewBag.HtmlMail = this.mailService.ReplaceTokens(plug.HtmlMail, fields).Replace("{DoubleOptinLink}", doubleOptinLink);
            ViewBag.TextMail = this.mailService.ReplaceTokens(plug.TextMail, fields);

            // get email addresses
            var useGlobalConfig = IsGlobalConfigEnabled();
            var from = this.mailHelper.GetEmailAddresses(this.configurationManager.Get<SendEmailPlugConfiguration>(c => c.From), plug.From, useGlobalConfig);
            var to = this.mailHelper.GetEmailAddresses(this.configurationManager.Get<SendEmailPlugConfiguration>(c => c.To), form.GetFieldValue(plug.To), useGlobalConfig);
            var cc = this.mailHelper.GetEmailAddresses(this.configurationManager.Get<SendEmailPlugConfiguration>(c => c.Cc), plug.Cc, useGlobalConfig);
            var bcc = this.mailHelper.GetEmailAddresses(this.configurationManager.Get<SendEmailPlugConfiguration>(c => c.Bcc), plug.Bcc, useGlobalConfig);
            var replyTo = this.mailHelper.GetEmailAddresses(this.configurationManager.Get<SendEmailPlugConfiguration>(c => c.ReplyTo), plug.ReplyTo, useGlobalConfig);

            // populate the mail
            return Populate(x =>
            {
                x.ViewName = this.presentationService.ResolveView(this.ControllerContext, "Mailers/SavePlug/DoubleOptin");
                x.Subject = ViewBag.Subject;

                // add addresses
                x.From = new MailAddress(from);
                if (!string.IsNullOrWhiteSpace(to)) to.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ForEach(address => x.To.Add(address));
                if (!string.IsNullOrWhiteSpace(cc)) cc.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ForEach(address => x.CC.Add(address));
                if (!string.IsNullOrWhiteSpace(bcc)) bcc.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ForEach(address => x.Bcc.Add(address));
                if (!string.IsNullOrWhiteSpace(replyTo)) x.ReplyToList.Add(replyTo);
            });
        }

        protected virtual bool IsGlobalConfigEnabled()
        {
            return Settings.GetBoolSetting(Definitions.Constants.AlwaysUseGlobalConfig, false);
        }
    }
}
