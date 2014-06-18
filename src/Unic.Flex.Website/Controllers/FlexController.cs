namespace Unic.Flex.Website.Controllers
{
    using System.Web.Mvc;
    using Unic.Flex.Context;
    using Unic.Flex.Mapping;
    using Unic.Flex.Model.ViewModel.Forms;
    using Unic.Flex.ModelBinding;
    using Unic.Flex.Plugs;
    using Unic.Flex.Presentation;

    public class FlexController : Controller
    {
        private readonly IPresentationService presentationService;

        private readonly IModelConverterService modelConverter;

        private readonly IContextService contextService;

        private readonly IUserDataRepository userDataRepository;

        private readonly IPlugsService plugsService;

        public FlexController(IPresentationService presentationService, IModelConverterService modelConverter, IContextService contextService, IUserDataRepository userDataRepository, IPlugsService plugsService)
        {
            this.presentationService = presentationService;
            this.modelConverter = modelConverter;
            this.contextService = contextService;
            this.userDataRepository = userDataRepository;
            this.plugsService = plugsService;
        }
        
        public ActionResult Form()
        {
            // get the form
            var form = ContextUtil.GetCurrentForm();
            if (form == null) return new EmptyResult();

            // get the current step
            var currentStep = form.GetActiveStep();
            if (currentStep == null) return new EmptyResult();

            // check if the current step may be accessed (only valid if all previous steps are done)
            // if step is not valid, redirect to the first step
            if (!this.contextService.IsStepAccessible(form, currentStep))
            {
                return this.Redirect(form.GetFirstStepUrl());
            }

            // revert step completion to the current step
            this.userDataRepository.RevertToStep(form.Id, currentStep.StepNumber);

            // return the view for this step
            var formView = this.presentationService.ResolveView(this.ControllerContext, form.ViewName);
            return this.View(formView, this.modelConverter.ConvertToViewModel(form));
        }

        [HttpPost]
        public ActionResult Form(FormViewModel model)
        {
            // get the current form
            var form = ContextUtil.GetCurrentForm();
            if (form == null) return new EmptyResult();
            
            // return view if we have any validation errors
            var formView = this.presentationService.ResolveView(this.ControllerContext, form.ViewName);
            if (!ModelState.IsValid)
            {
                return this.View(formView, model);    
            }

            // get the current step
            var currentStep = form.GetActiveStep();
            if (currentStep == null) return new EmptyResult();

            // store the values in the session redirect to next step if we have a next step
            this.contextService.StoreFormValues(form, model);
            var nextStepUrl = currentStep.GetNextStepUrl();
            if (!string.IsNullOrWhiteSpace(nextStepUrl))
            {
                this.userDataRepository.CompleteStep(form.Id, currentStep.StepNumber);
                return this.Redirect(nextStepUrl);
            }

            // execute save plugs
            this.plugsService.ExecuteSavePlugs(form);

            // clear session values
            this.userDataRepository.ClearForm(form.Id);

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