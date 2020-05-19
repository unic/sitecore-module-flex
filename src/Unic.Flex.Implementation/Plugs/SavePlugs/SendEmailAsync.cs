namespace Unic.Flex.Implementation.Plugs.SavePlugs
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Configuration;
    using Core.Mailing;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Mailers;
    using MimeKit;
    using Unic.Flex.Model.Forms;

    /// <summary>
    /// Send email async save plug model
    /// </summary>
    [SitecoreType(TemplateId = "{A63884CE-54F1-4134-8A29-05017D5317F6}")]
    public class SendEmailAsync : SendEmail
    {
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
                return true;
            }
        }

        public SendEmailAsync(IMailRepository mailRepository, ISavePlugMailer savePlugMailer) : base(mailRepository, savePlugMailer)
        {
        }

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

            var mailMessageByConfiguration =
                this.savePlugMailer.GetMailMessageByConfiguration(form, this);

            this.ApplyConfigurationOnMessage(mailMessageByConfiguration, message);

            this.mailRepository.SendMail(message);
        }
    }
}
