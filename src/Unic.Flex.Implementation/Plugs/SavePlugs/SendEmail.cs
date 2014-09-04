namespace Unic.Flex.Implementation.Plugs.SavePlugs
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Sitecore.Diagnostics;
    using Unic.Flex.Implementation.Mailers;
    using Unic.Flex.Mailing;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Global;
    using Unic.Flex.Model.DomainModel.Plugs.SavePlugs;
    using Unic.Flex.Model.GlassExtensions.Attributes;

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
        /// Executes the save plug.
        /// </summary>
        /// <param name="form">The form.</param>
        public override void Execute(Form form)
        {
            Assert.ArgumentNotNull(form, "form");

            var mailMessage = this.savePlugMailer.GetMessage(form, this);
            this.mailRepository.SendMail(mailMessage);
        }
    }
}
