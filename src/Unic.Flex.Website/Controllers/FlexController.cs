namespace Unic.Flex.Website.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Unic.Flex.Context;
    using Unic.Flex.Definitions;
    using Unic.Flex.Logging;
    using Unic.Flex.Mapping;
    using Unic.Flex.Model.Exceptions;
    using Unic.Flex.Model.Validation;
    using Unic.Flex.Model.ViewModel.Components;
    using Unic.Flex.Model.ViewModel.Forms;
    using Unic.Flex.ModelBinding;
    using Unic.Flex.Plugs;
    using Unic.Flex.Presentation;
    using Unic.Profiling;

    public class FlexController : Controller
    {
        private const string ProfileGetEventName = "Flex :: GET Controller Action";

        private const string ProfilePostEventName = "Flex :: POST Controller Action";
        
        private readonly IPresentationService presentationService;

        private readonly IModelConverterService modelConverter;

        private readonly IContextService contextService;

        private readonly IUserDataRepository userDataRepository;

        private readonly IPlugsService plugsService;

        private readonly IFlexContext flexContext;

        private readonly IUrlService urlService;

        private readonly IFormRepository formRepository;

        private readonly ILogger logger;

        private readonly IExceptionState exceptionState;

        public FlexController(IPresentationService presentationService, IModelConverterService modelConverter, IContextService contextService, IUserDataRepository userDataRepository, IPlugsService plugsService, IFlexContext flexContext, IUrlService urlService, IFormRepository formRepository, ILogger logger, IExceptionState exceptionState)
        {
            this.presentationService = presentationService;
            this.modelConverter = modelConverter;
            this.contextService = contextService;
            this.userDataRepository = userDataRepository;
            this.plugsService = plugsService;
            this.flexContext = flexContext;
            this.urlService = urlService;
            this.formRepository = formRepository;
            this.logger = logger;
            this.exceptionState = exceptionState;
        }

        public ActionResult Form()
        {
            Profiler.OnStart(this, ProfileGetEventName);
            
            // get the form
            var form = this.flexContext.Form;
            if (form == null) return new EmptyResult();

            // get the current step
            var currentStep = form.GetActiveStep();
            if (currentStep == null) return new EmptyResult();

            // show errors
            if (this.exceptionState.HasErrors)
            {
                var result = this.ShowError();
                Profiler.OnEnd(this, ProfileGetEventName);
                return result;
            }

            // check if we need to show the success message
            if (Request.QueryString[Constants.SuccessQueryStringKey] == Constants.SuccessQueryStringValue
                && currentStep.StepNumber == form.Steps.Count()
                && !this.userDataRepository.IsFormStored(form.Id))
            {
                var result = this.ShowSuccessMessage(form.SuccessMessage);
                Profiler.OnEnd(this, ProfileGetEventName);
                return result;
            }

            // check if we need to cancel the current form
            if (Request.QueryString[Constants.CancelQueryStringKey] == Constants.CancelQueryStringValue
                && form.CancelLink != null
                && !string.IsNullOrWhiteSpace(form.CancelLink.Url))
            {
                // clear session values
                this.userDataRepository.ClearForm(form.Id);

                // redirect to correct url
                Profiler.OnEnd(this, ProfileGetEventName);
                return this.Redirect(form.CancelLink.Url);
            }

            // check if the current step may be accessed (only valid if all previous steps are done)
            // if step is not valid, redirect to the first step
            if (!this.contextService.IsStepAccessible(form, currentStep))
            {
                Profiler.OnEnd(this, ProfileGetEventName);
                return this.Redirect(form.GetFirstStepUrl());
            }

            // revert step completion to the current step
            this.userDataRepository.RevertToStep(form.Id, currentStep.StepNumber);

            // return the view for this step
            var formViewModel = this.modelConverter.ConvertToViewModel(form);
            var formView = this.presentationService.ResolveView(this.ControllerContext, formViewModel);
            Profiler.OnEnd(this, ProfileGetEventName);
            return this.View(formView, formViewModel);
        }

        [HttpPost]
        public ActionResult Form(IFormViewModel model)
        {
            Profiler.OnStart(this, ProfilePostEventName);
            
            // get the current form
            var form = this.flexContext.Form;
            if (form == null) return new EmptyResult();

            // get the current step
            var currentStep = form.GetActiveStep();
            if (currentStep == null) return new EmptyResult();

            // check if the current step may be accessed (only valid if all previous steps are done)
            // if step is not valid, redirect to the first step
            if (!this.contextService.IsStepAccessible(form, currentStep))
            {
                Profiler.OnEnd(this, ProfilePostEventName);
                return this.Redirect(form.GetFirstStepUrl());
            }

            // return view if we have any validation errors
            var formView = this.presentationService.ResolveView(this.ControllerContext, model);
            if (!ModelState.IsValid)
            {
                Profiler.OnEnd(this, ProfilePostEventName);
                return this.View(formView, model);
            }

            // store the values in the session redirect to next step if we have a next step
            this.contextService.StoreFormValues(form, model);
            var nextStepUrl = currentStep.GetNextStepUrl();
            if (!string.IsNullOrWhiteSpace(nextStepUrl))
            {
                this.userDataRepository.CompleteStep(form.Id, currentStep.StepNumber);
                Profiler.OnEnd(this, ProfilePostEventName);
                return this.Redirect(nextStepUrl);
            }

            // execute save plugs
            this.plugsService.ExecuteSavePlugs(form);

            // clear session values
            this.userDataRepository.ClearForm(form.Id);

            // redirect to the sucess page
            if (form.SuccessRedirect != null && !string.IsNullOrWhiteSpace(form.SuccessRedirect.Url))
            {
                Profiler.OnEnd(this, ProfilePostEventName);
                return this.Redirect(form.SuccessRedirect.Url);
            }

            // redirect to the current page and show the success message
            Profiler.OnEnd(this, ProfilePostEventName);
            return this.Redirect(this.urlService.AddQueryStringToCurrentUrl(Constants.SuccessQueryStringKey, Constants.SuccessQueryStringValue));
        }

        /// <summary>
        /// Ajaxes the validator.
        /// </summary>
        /// <param name="validator">The validator.</param>
        /// <returns>Boolean value as json result</returns>
        public ActionResult AjaxValidator(Guid validator)
        {
            // get the validator
            var validatorItem = this.formRepository.LoadValidator(validator) as AjaxValidator;
            if (validatorItem == null)
            {
                this.logger.Warn(string.Format("Ajax validator with guid '{0}' not found", validator), this);
                return this.Json(false, JsonRequestBehavior.AllowGet);
            }

            // get the key of the valud
            var collection = validatorItem.HttpMethod == FormMethod.Post ? Request.Form : Request.QueryString;
            var key = collection.AllKeys.FirstOrDefault(x => x.EndsWith("Value"));
            if (string.IsNullOrWhiteSpace(key))
            {
                this.logger.Warn(string.Format("No valid value provided to check ajax validator '{0}'", validator), this);
                return this.Json(false, JsonRequestBehavior.AllowGet);
            }
            
            // validate
            return this.Json(validatorItem.IsValid(collection[key]), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Removes the uploaded file from the user data storage.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="field">The field.</param>
        /// <returns>Result for handling within the frontend</returns>
        public ActionResult RemoveUploadedFile(string form, string field)
        {
            if (this.userDataRepository.IsFieldStored(form, field))
            {
                this.userDataRepository.SetValue(form, field, null);
            }

            return this.Content("OK"); // todo: return whatever is needed from the frontend
        }

        private ActionResult ShowError()
        {
            var model = new ErrorViewModel { Messages = this.exceptionState.Messages };
            var view = this.presentationService.ResolveView(this.ControllerContext, model);
            return this.View(view, model);
        }

        private ActionResult ShowSuccessMessage(string message)
        {
            var model = new SuccessMessageViewModel { Message = message };
            var view = this.presentationService.ResolveView(this.ControllerContext, model);
            return this.View(view, model);
        }
    }
}