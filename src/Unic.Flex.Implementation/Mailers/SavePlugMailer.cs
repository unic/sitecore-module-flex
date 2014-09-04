namespace Unic.Flex.Implementation.Mailers
{
    using Castle.Core.Internal;
    using Mvc.Mailer;
    using System;
    using System.Linq;
    using System.Net.Mail;
    using System.Web;
    using Unic.Configuration;
    using Unic.Flex.Implementation.Configuration;
    using Unic.Flex.Implementation.Fields.InputFields;
    using Unic.Flex.Implementation.Plugs.SavePlugs;
    using Unic.Flex.Mailing;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Presentation;

    public class SavePlugMailer : MailerBase, ISavePlugMailer
    {
        private readonly IPresentationService presentationService;

        private readonly IConfigurationManager configurationManager;

        private readonly IMailService mailService;

        private string theme;

        public SavePlugMailer(IPresentationService presentationService, IConfigurationManager configurationManager, IMailService mailService)
        {
            this.presentationService = presentationService;
            this.configurationManager = configurationManager;
            this.mailService = mailService;
        }

        public virtual MvcMailMessage GetMessage(Form form, SendEmail plug)
        {
            // ensure the mailer has been initialized
            if (this.ControllerContext == null)
            {
                this.Initialize(HttpContext.Current.Request.RequestContext);
            }

            // set the theme
            this.theme = plug.Theme != null ? plug.Theme.Value : string.Empty;

            // get the layouts
            this.ViewBag.HtmlLayout = this.presentationService.ResolveView(this.ControllerContext, "Mailers/_Layout", this.theme);
            this.ViewBag.TextLayout = this.presentationService.ResolveView(this.ControllerContext, "Mailers/_Layout.text", this.theme);
            
            // add data
            this.ViewBag.Form = form;
            this.ViewBag.Theme = this.theme;

            // add content
            var fields = form.GetSections().SelectMany(s => s.Fields).Where(f => f.ShowInSummary).ToList();
            this.ViewBag.HtmlMailIntroduction = this.mailService.ReplaceTokens(plug.HtmlMailIntroduction, fields);
            this.ViewBag.HtmlMailFooter = this.mailService.ReplaceTokens(plug.HtmlMailFooter, fields);
            this.ViewBag.TextMailIntroduction = this.mailService.ReplaceTokens(plug.TextMailIntroduction, fields);
            this.ViewBag.TextMailFooter = this.mailService.ReplaceTokens(plug.TextMailFooter, fields);

            // todo: add receiver field -> if the "to" address depends on a dropdown in the text (i.e. a "topics" field)

            // todo: add reply-to address -> if the user has entered an email address in the form, this should be the reply-to address

            // get email addresses
            var from = this.GetEmailAddresses(this.configurationManager.Get<SendEmailPlugConfiguration>(c => c.From), plug.From);
            var to = this.GetEmailAddresses(this.configurationManager.Get<SendEmailPlugConfiguration>(c => c.To), plug.To);
            var cc = this.GetEmailAddresses(this.configurationManager.Get<SendEmailPlugConfiguration>(c => c.Cc), plug.Cc);
            var bcc = this.GetEmailAddresses(this.configurationManager.Get<SendEmailPlugConfiguration>(c => c.Bcc), plug.Bcc);
            
            // populate the mail
            return this.Populate(x =>
            {
                x.ViewName = this.presentationService.ResolveView(this.ControllerContext, "Mailers/SavePlug/Form", this.theme);
                x.Subject = this.mailService.ReplaceTokens(plug.Subject, fields);

                // add addresses
                x.From = new MailAddress(from);
                to.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ForEach(address => x.To.Add(address));
                if (!string.IsNullOrWhiteSpace(cc)) cc.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ForEach(address => x.CC.Add(address));
                if (!string.IsNullOrWhiteSpace(bcc)) bcc.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ForEach(address => x.Bcc.Add(address));

                // add attachments
                foreach (var fileField in fields
                    .OfType<FileUploadField>()
                    .Where(field => field.Value != null))
                {
                    x.Attachments.Add(new Attachment(fileField.Value.InputStream, fileField.Value.FileName));
                }
            });
        }

        /// <summary>
        /// Get the text view of this mail.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns>Path to the text view</returns>
        public override string TextViewName(string viewName)
        {
            return this.presentationService.ResolveView(this.ControllerContext, "Mailers/SavePlug/Form.text", this.theme);
        }

        /// <summary>
        /// Gets the email addresses and replace settings parameter from config file.
        /// </summary>
        /// <param name="globalConfig">The email address from the global configuration.</param>
        /// <param name="plugConfig">The email address from the plug configuration.</param>
        /// <returns>The final email addresses</returns>
        private string GetEmailAddresses(string globalConfig, string plugConfig)
        {
            return this.mailService.ReplaceEmailAddressesFromConfig(!string.IsNullOrWhiteSpace(plugConfig) ? plugConfig : globalConfig);
        }
    }
}
