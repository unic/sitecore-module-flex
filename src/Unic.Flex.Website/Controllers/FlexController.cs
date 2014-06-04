namespace Unic.Flex.Website.Controllers
{
    using System.Web.Mvc;
    using Unic.Flex.Context;
    using Unic.Flex.Mapping;
    using Unic.Flex.Model.ViewModel.Forms;
    using Unic.Flex.ModelBinding;
    using Unic.Flex.Presentation;

    public class FlexController : Controller
    {
        private readonly IPresentationService presentationService;

        private readonly IModelConverterService modelConverter;

        private readonly IContextService contextService;

        public FlexController(IPresentationService presentationService, IModelConverterService modelConverter, IContextService contextService)
        {
            this.presentationService = presentationService;
            this.modelConverter = modelConverter;
            this.contextService = contextService;
        }
        
        public ActionResult Form()
        {
            var form = ContextUtil.GetCurrentForm();
            if (form == null) return new EmptyResult();

            var formView = this.presentationService.ResolveView(this.ControllerContext, form.ViewName);

            // todo: try to add the @Html.Sitecore.BeginForm() and @Html.Sitecore.FormHandler() for the form post

            return this.View(formView, this.modelConverter.ConvertToViewModel(form));
        }

        [HttpPost]
        public ActionResult Form(FormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View("~/Views/Modules/Flex/Default/Form.cshtml", model);    
            }

            var form = ContextUtil.GetCurrentForm();
            this.contextService.StoreFormValues(form, model);
            var nextStepUrl = form.GetActiveStep().GetNextStepUrl();
            if (!string.IsNullOrWhiteSpace(nextStepUrl))
            {
                return this.Redirect(nextStepUrl);
            }

            return Content("this was the last step, form has been submitted");
        }
    }
}