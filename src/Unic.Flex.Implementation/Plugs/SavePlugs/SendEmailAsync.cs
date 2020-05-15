namespace Unic.Flex.Implementation.Plugs.SavePlugs
{
    using Core.Mailing;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Mailers;

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
    }
}
