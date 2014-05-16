namespace Unic.Flex.Website.Controllers
{
    using System.Web.Mvc;
    using Unic.Flex.Context;
    using Unic.Flex.Presentation;
    using Unic.Flex.Website.ModelBinding;
    using Unic.Flex.Website.Models.Flex;

    public class FlexController : Controller
    {
        private readonly IPresentationService presentationService;

        public FlexController(IPresentationService presentationService)
        {
            this.presentationService = presentationService;
        }
        
        public ActionResult Form()
        {
            var form = FlexContext.Current.Form;
            if (form == null) return new EmptyResult();

            var formView = this.presentationService.ResolveView(this.ControllerContext, form.ViewName);

            return this.View(formView, form.ToViewModel());
        }

        [HttpPost]
        public ActionResult Form(FormViewModel model)
        {
            return this.View("~/Views/Modules/Flex//Default/Form.cshtml", model);
        }
    }
}