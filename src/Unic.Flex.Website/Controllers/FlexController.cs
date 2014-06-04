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

        private readonly IUserDataRepository userDataRepository;

        public FlexController(IPresentationService presentationService, IModelConverterService modelConverter, IContextService contextService, IUserDataRepository userDataRepository)
        {
            this.presentationService = presentationService;
            this.modelConverter = modelConverter;
            this.contextService = contextService;
            this.userDataRepository = userDataRepository;
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

            // todo: execute save plugs

            // clear session values
            this.userDataRepository.ClearForm(form.ItemId.ToString());

            // redirect to the sucess page
            if (form.SuccessRedirect != null && !string.IsNullOrWhiteSpace(form.SuccessRedirect.Url))
            {
                return this.Redirect(form.SuccessRedirect.Url);
            }

            // todo: a 301 should be done before showing the success message, to prevent multiple postbacks of the complete form
            return this.Content(form.SuccessMessage);
        }
    }
}