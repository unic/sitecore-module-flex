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

            this.ApplyGlobalConfigurationOnMessage(mailMessageByConfiguration, message);

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

        private MimeMessage ApplyGlobalConfigurationOnMessage(MailMessageGlobalConfiguration messageGlobalConfiguration, MimeMessage message)
        {
            if (messageGlobalConfiguration.From != null)
            {
                message.From.Clear();
                message.Headers.Replace(HeaderId.From, string.Empty);
                message.From.Add((InternetAddress)(MailboxAddress)messageGlobalConfiguration.From);
            }
            if (messageGlobalConfiguration.ReplyTo.Count > 0)
            {
                message.ReplyTo.Clear();
                message.Headers.Replace(HeaderId.ReplyTo, string.Empty);
                message.ReplyTo.AddRange((IEnumerable<InternetAddress>)(InternetAddressList)messageGlobalConfiguration.ReplyTo);
            }
            if (messageGlobalConfiguration.To.Count > 0)
            {
                message.To.Clear();
                message.Headers.Replace(HeaderId.To, string.Empty);
                message.To.AddRange((IEnumerable<InternetAddress>)(InternetAddressList)messageGlobalConfiguration.To);
            }
            if (messageGlobalConfiguration.Cc.Count > 0)
            {
                message.Cc.Clear();
                message.Headers.Replace(HeaderId.Cc, string.Empty);
                message.Cc.AddRange((IEnumerable<InternetAddress>)(InternetAddressList)messageGlobalConfiguration.Cc);
            }
            if (messageGlobalConfiguration.Bcc.Count > 0)
            {
                message.Bcc.Clear();
                message.Headers.Replace(HeaderId.Bcc, string.Empty);
                message.Bcc.AddRange((IEnumerable<InternetAddress>)(InternetAddressList)messageGlobalConfiguration.Bcc);
            }

            return message;
        }
    }
}
