using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Implementation.Mailers
{
    using System.Net.Mail;
    using System.Web;
    using Mvc.Mailer;
    using Unic.Flex.Presentation;

    public class SavePlugMailer : MailerBase, ISavePlugMailer
    {
        private readonly IPresentationService presentationService;

        public SavePlugMailer(IPresentationService presentationService)
        {
            this.presentationService = presentationService;
        }

        public virtual MvcMailMessage GetMessage()
        {
            // ensure the mailer has been initialized
            if (this.ControllerContext == null)
            {
                this.Initialize(HttpContext.Current.Request.RequestContext);
            }
            
            // get the html part
            this.ViewBag.HtmlBody = "this is my html body";
            this.ViewBag.HtmlLayout = this.presentationService.ResolveView(this.ControllerContext, "Mailers/_Layout");

            // get the text part
            this.ViewBag.TextBody = "this is my plain text body";
            this.ViewBag.TextLayout = this.presentationService.ResolveView(this.ControllerContext, "Mailers/_Layout.text");
            
            // populate the mail
            return this.Populate(x =>
            {
                x.ViewName = this.presentationService.ResolveView(this.ControllerContext, "Mailers/SavePlug/Form");
                x.Subject = "Test email";
                x.From = new MailAddress("noreply@post.ch");
                x.To.Add("kevin.brechbuehl@unic.com");
            });
        }

        /// <summary>
        /// Get the text view of this mail.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns>Path to the text view</returns>
        public override string TextViewName(string viewName)
        {
            return this.presentationService.ResolveView(this.ControllerContext, "Mailers/SavePlug/Form.text");
        }
    }
}
