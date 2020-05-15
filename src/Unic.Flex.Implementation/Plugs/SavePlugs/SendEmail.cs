namespace Unic.Flex.Implementation.Plugs.SavePlugs
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Text;
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using MimeKit;
    using Unic.Flex.Core.Mailing;
    using Unic.Flex.Implementation.Mailers;
    using Unic.Flex.Model.Fields;
    using Unic.Flex.Model.Forms;
    using Unic.Flex.Model.GlassExtensions.Attributes;
    using Unic.Flex.Model.Plugs;
    using Unic.Flex.Model.Specifications;

    /// <summary>
    /// Send email save plug model
    /// </summary>
    [SitecoreType(TemplateId = "{DF5C2C2F-4A48-4206-9DFB-0DCDE27E2233}")]
    public class SendEmail : SavePlugBase
    {
        /// <summary>
        /// The mail repository
        /// </summary>
        private readonly IMailRepository mailRepository;

        /// <summary>
        /// The save plug mailer
        /// </summary>
        private readonly ISavePlugMailer savePlugMailer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendEmail"/> class.
        /// </summary>
        /// <param name="mailRepository">The mail repository.</param>
        /// <param name="savePlugMailer">The save plug mailer.</param>
        public SendEmail(IMailRepository mailRepository, ISavePlugMailer savePlugMailer)
        {
            this.mailRepository = mailRepository;
            this.savePlugMailer = savePlugMailer;
        }

        /// <summary>
        /// Gets a value indicating whether this plug should be executed asynchronous. Because we use MvcMailer we can't use this in
        /// a background process.
        /// </summary>
        /// <value>
        /// <c>true</c> if this plug should be executed asynchronous; otherwise, <c>false</c>.
        /// </value>
        public override bool IsAsync
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the theme.
        /// </summary>
        /// <value>
        /// The theme.
        /// </value>
        [SitecoreSharedField("Theme")]
        public virtual Specification Theme { get; set; }

        /// <summary>
        /// Gets or sets the from address.
        /// </summary>
        /// <value>
        /// The from address.
        /// </value>
        [SitecoreField("From")]
        public virtual string From { get; set; }

        /// <summary>
        /// Gets or sets the reply to.
        /// </summary>
        /// <value>
        /// The reply to.
        /// </value>
        [SitecoreField("Reply To")]
        public virtual string ReplyTo { get; set; }

        /// <summary>
        /// Gets or sets the to address.
        /// </summary>
        /// <value>
        /// The to address.
        /// </value>
        [SitecoreField("To")]
        public virtual string To { get; set; }

        /// <summary>
        /// Gets or sets the CC address.
        /// </summary>
        /// <value>
        /// The CC address.
        /// </value>
        [SitecoreField("Cc")]
        public virtual string Cc { get; set; }

        /// <summary>
        /// Gets or sets the BCC address.
        /// </summary>
        /// <value>
        /// The BCC address.
        /// </value>
        [SitecoreField("Bcc")]
        public virtual string Bcc { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        [SitecoreField("Subject")]
        public virtual string Subject { get; set; }

        /// <summary>
        /// Gets or sets the gender field for token with Salutation: {Salutation}
        /// </summary>
        [SitecoreReusableField("Gender Field For Salutation", Setting = SitecoreFieldSettings.InferType)]
        public virtual IField GenderField { get; set; }

        /// <summary>
        /// Gets or sets the salutation mappings depending on the gender value
        /// </summary>
        [SitecoreField("Gender Salutation Mapping")]
        public virtual NameValueCollection GenderSalutationMapping { get; set; }

        /// <summary>
        /// Gets or sets the HTML mail introduction.
        /// </summary>
        /// <value>
        /// The HTML mail introduction.
        /// </value>
        [SitecoreField("Html Mail Introduction")]
        public virtual string HtmlMailIntroduction { get; set; }

        /// <summary>
        /// Gets or sets the HTML mail footer.
        /// </summary>
        /// <value>
        /// The HTML mail footer.
        /// </value>
        [SitecoreField("Html Mail Footer")]
        public virtual string HtmlMailFooter { get; set; }

        /// <summary>
        /// Gets or sets the text mail introduction.
        /// </summary>
        /// <value>
        /// The text mail introduction.
        /// </value>
        [SitecoreField("Text Mail Introduction")]
        public virtual string TextMailIntroduction { get; set; }

        /// <summary>
        /// Gets or sets the text mail footer.
        /// </summary>
        /// <value>
        /// The text mail footer.
        /// </value>
        [SitecoreField("Text Mail Footer")]
        public virtual string TextMailFooter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to send attachments in the mail or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if attachments should be send; otherwise, <c>false</c>.
        /// </value>
        [SitecoreField("Send Attachments")]
        public virtual bool SendAttachments { get; set; }

        /// <summary>
        /// Gets or sets the receiver fields.
        /// </summary>
        /// <value>
        /// The receiver fields.
        /// </value>
        [SitecoreReusableField("Receiver Fields", Setting = SitecoreFieldSettings.InferType)]
        public virtual IEnumerable<IField> ReceiverFields { get; set; }

        /// <summary>
        /// Gets or sets the receiver email mapping.
        /// </summary>
        /// <value>
        /// The receiver email mapping.
        /// </value>
        [SitecoreField("Receiver Email Mapping")]
        public virtual NameValueCollection ReceiverEmailMapping { get; set; }

        /// <summary>
        /// Gets or sets the reply to fields.
        /// </summary>
        /// <value>
        /// The reply to fields.
        /// </value>
        [SitecoreReusableField("Reply To Fields", Setting = SitecoreFieldSettings.InferType)]
        public virtual IEnumerable<IField> ReplyToFields { get; set; }

        /// <summary>
        /// Executes the save plug.
        /// </summary>
        /// <param name="form">The form.</param>
        public override void Execute(IForm form)
        {
            MimeMessage message;
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(this.TaskData)))
            {
                message = MimeMessage.Load(stream);
            }

            this.mailRepository.SendMail(message);
        }

        public override string GetTaskDataForStorage(IForm form)
        {
            var mailMessage = this.savePlugMailer.GetMessage(form, this);
            var mimeMessage = MimeMessage.CreateFromMailMessage(mailMessage);
            string serializedMimeMessage;
            using (var stream = new MemoryStream())
            {
                mimeMessage.WriteTo(stream);
                serializedMimeMessage = Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);
            }

            return serializedMimeMessage;
        }
    }
}
