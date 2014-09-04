namespace Unic.Flex.Implementation.Mailers
{
    using System.Linq;
    using System.Net.Mail;
    using System.Web;
    using Mvc.Mailer;
    using Unic.Configuration;
    using Unic.Flex.Implementation.Configuration;
    using Unic.Flex.Implementation.Fields.InputFields;
    using Unic.Flex.Model.Configuration;
    using Unic.Flex.Model.Configuration.Extensions;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Presentation;

    public class SavePlugMailer : MailerBase, ISavePlugMailer
    {
        private readonly IPresentationService presentationService;

        private readonly IConfigurationManager configurationManager;

        private string theme;

        public SavePlugMailer(IPresentationService presentationService, IConfigurationManager configurationManager)
        {
            this.presentationService = presentationService;
            this.configurationManager = configurationManager;
        }

        public virtual MvcMailMessage GetMessage(Form form, string theme)
        {
            this.theme = theme;
            
            // ensure the mailer has been initialized
            if (this.ControllerContext == null)
            {
                this.Initialize(HttpContext.Current.Request.RequestContext);
            }

            // add data
            this.ViewBag.Form = form;
            this.ViewBag.Theme = theme;
            
            // get the layouts
            this.ViewBag.HtmlLayout = this.presentationService.ResolveView(this.ControllerContext, "Mailers/_Layout", this.theme);
            this.ViewBag.TextLayout = this.presentationService.ResolveView(this.ControllerContext, "Mailers/_Layout.text", this.theme);
            
            // populate the mail
            return this.Populate(x =>
            {
                x.ViewName = this.presentationService.ResolveView(this.ControllerContext, "Mailers/SavePlug/Form", this.theme);
                x.Subject = "Test email";
                x.From = new MailAddress("noreply@post.ch");
                x.To.Add("kevin.brechbuehl@unic.com");

                // add attachments
                foreach (var fileField in form
                    .GetSections()
                    .SelectMany(section => section.Fields)
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
    }
}
