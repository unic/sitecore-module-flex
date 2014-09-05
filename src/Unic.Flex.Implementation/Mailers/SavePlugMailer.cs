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
    using Unic.Flex.Implementation.Validators;
    using Unic.Flex.Mailing;
    using Unic.Flex.Model.DomainModel.Fields;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Presentation;

    /// <summary>
    /// Mailer implementation for the send email plug
    /// </summary>
    public class SavePlugMailer : MailerBase, ISavePlugMailer
    {
        /// <summary>
        /// The presentation service
        /// </summary>
        private readonly IPresentationService presentationService;

        /// <summary>
        /// The configuration manager
        /// </summary>
        private readonly IConfigurationManager configurationManager;

        /// <summary>
        /// The mail service
        /// </summary>
        private readonly IMailService mailService;

        /// <summary>
        /// The theme
        /// </summary>
        private string theme;

        /// <summary>
        /// Initializes a new instance of the <see cref="SavePlugMailer"/> class.
        /// </summary>
        /// <param name="presentationService">The presentation service.</param>
        /// <param name="configurationManager">The configuration manager.</param>
        /// <param name="mailService">The mail service.</param>
        public SavePlugMailer(IPresentationService presentationService, IConfigurationManager configurationManager, IMailService mailService)
        {
            this.presentationService = presentationService;
            this.configurationManager = configurationManager;
            this.mailService = mailService;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="plug">The plug.</param>
        /// <returns>
        /// Message to be sent over the mvc mailer
        /// </returns>
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

            // get email addresses
            var from = this.GetEmailAddresses(this.configurationManager.Get<SendEmailPlugConfiguration>(c => c.From), plug.From);
            var cc = this.GetEmailAddresses(this.configurationManager.Get<SendEmailPlugConfiguration>(c => c.Cc), plug.Cc);
            var bcc = this.GetEmailAddresses(this.configurationManager.Get<SendEmailPlugConfiguration>(c => c.Bcc), plug.Bcc);
            var replyTo = this.GetEmailFromField(plug.ReplyToField, form);

            // get the receiver email address
            var to = this.GetMappedEmailFromField(plug.ReceiverField, form, plug);
            if (string.IsNullOrWhiteSpace(to))
            {
                to = this.GetEmailAddresses(this.configurationManager.Get<SendEmailPlugConfiguration>(c => c.To), plug.To);
            }
            
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
                if (!string.IsNullOrWhiteSpace(replyTo)) x.ReplyToList.Add(replyTo);

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
        /// Gets the email address from a field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="form">The form.</param>
        /// <returns>The email if valid, or empty string</returns>
        private string GetEmailFromField(IField field, Form form)
        {
            var fieldValue = this.GetFieldValue(field, form);
            return (!string.IsNullOrWhiteSpace(fieldValue) && (new EmailValidator()).IsValid(fieldValue)) ? fieldValue : string.Empty;
        }

        /// <summary>
        /// Gets the mapped email from the referenced field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="form">The form.</param>
        /// <param name="plug">The plug.</param>
        /// <returns>The email address found in the mapping if available.</returns>
        private string GetMappedEmailFromField(IField field, Form form, SendEmail plug)
        {
            var fieldValue = this.GetFieldValue(field, form);
            if (string.IsNullOrWhiteSpace(fieldValue)) return string.Empty;

            var email = plug.ReceiverEmailMapping.Get(fieldValue);
            return (!string.IsNullOrWhiteSpace(email) && (new EmailValidator()).IsValid(email)) ? email : string.Empty;
        }

        /// <summary>
        /// Gets the value of a field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="form">The form.</param>
        /// <returns>The value from the field as a string</returns>
        private string GetFieldValue(IField field, Form form)
        {
            if (field == null) return string.Empty;
            var formField = form.GetSections().SelectMany(s => s.Fields).FirstOrDefault(f => f.ItemId == field.ItemId);
            return formField != null ? formField.Value as string : string.Empty;
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
