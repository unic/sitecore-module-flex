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
    using Unic.Flex.Plugs;
    using Unic.Flex.Presentation;
    using Unic.Profiling;

    /// <summary>
    /// The one and only controller for Flex
    /// </summary>
    public class FlexController : Controller
    {
        /// <summary>
        /// Profiling event namef or GET method
        /// </summary>
        private const string ProfileGetEventName = "Flex :: GET Controller Action";

        /// <summary>
        /// Profiling event namef or POST method
        /// </summary>
        private const string ProfilePostEventName = "Flex :: POST Controller Action";

        #region Fields

        /// <summary>
        /// The presentation service
        /// </summary>
        private readonly IPresentationService presentationService;

        /// <summary>
        /// The model converter
        /// </summary>
        private readonly IModelConverterService modelConverter;

        /// <summary>
        /// The context service
        /// </summary>
        private readonly IContextService contextService;

        /// <summary>
        /// The user data repository
        /// </summary>
        private readonly IUserDataRepository userDataRepository;

        /// <summary>
        /// The plugs service
        /// </summary>
        private readonly IPlugsService plugsService;

        /// <summary>
        /// The flex context
        /// </summary>
        private readonly IFlexContext flexContext;

        /// <summary>
        /// The URL service
        /// </summary>
        private readonly IUrlService urlService;

        /// <summary>
        /// The form repository
        /// </summary>
        private readonly IFormRepository formRepository;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The exception state
        /// </summary>
        private readonly IExceptionState exceptionState;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="FlexController"/> class.
        /// </summary>
        /// <param name="presentationService">The presentation service.</param>
        /// <param name="modelConverter">The model converter.</param>
        /// <param name="contextService">The context service.</param>
        /// <param name="userDataRepository">The user data repository.</param>
        /// <param name="plugsService">The plugs service.</param>
        /// <param name="flexContext">The flex context.</param>
        /// <param name="urlService">The URL service.</param>
        /// <param name="formRepository">The form repository.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="exceptionState">State of the exception.</param>
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

        /// <summary>
        /// Controller action for GET requests
        /// </summary>
        /// <returns>Result of this action.</returns>
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

        /// <summary>
        /// Controller action for POST requests.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Result of the action.</returns>
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

            // show errors
            if (this.exceptionState.HasErrors)
            {
                var result = this.ShowError();
                Profiler.OnEnd(this, ProfilePostEventName);
                return result;
            }

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

        /// <summary>
        /// Shows the error.
        /// </summary>
        /// <returns>View for showing errors</returns>
        private ActionResult ShowError()
        {
            var model = new ErrorViewModel { Messages = this.exceptionState.Messages };
            var view = this.presentationService.ResolveView(this.ControllerContext, model);
            return this.View(view, model);
        }

        /// <summary>
        /// Shows the success message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>View for showing the success message</returns>
        private ActionResult ShowSuccessMessage(string message)
        {
            var model = new SuccessMessageViewModel { Message = message };
            var view = this.presentationService.ResolveView(this.ControllerContext, model);
            return this.View(view, model);
        }
    }
}