namespace Unic.Flex.Website.Controllers
{
    using Glass.Mapper.Sc;
    using Sitecore;
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography;
    using System.Web.Mvc;
    using Unic.Configuration;
    using Unic.Flex.Core.Attributes;
    using Unic.Flex.Core.Context;
    using Unic.Flex.Core.Globalization;
    using Unic.Flex.Core.Logging;
    using Unic.Flex.Core.Mapping;
    using Unic.Flex.Core.Plugs;
    using Unic.Flex.Core.Presentation;
    using Unic.Flex.Core.Utilities;
    using Unic.Flex.Implementation.Database;
    using Unic.Flex.Implementation.Fields.InputFields;
    using Unic.Flex.Model.Configuration;
    using Unic.Flex.Model.Validation;
    using Unic.Flex.Model.ViewModel.Components;
    using Unic.Flex.Model.ViewModel.Forms;
    using Constants = Unic.Flex.Core.Definitions.Constants;
    using DependencyResolver = Unic.Flex.Core.DependencyInjection.DependencyResolver;
    using Profiler = Unic.Profiling.Profiler;
    using Settings = Sitecore.Configuration.Settings;

    /// <summary>
    /// The one and only controller for Flex
    /// </summary>
    public class FlexController : Controller
    {
        /// <summary>
        /// Profiling event name for GET method
        /// </summary>
        private const string ProfileGetEventName = "Flex :: GET Controller Action";

        /// <summary>
        /// Profiling event name for POST method
        /// </summary>
        private const string ProfilePostEventName = "Flex :: POST Controller Action";

        /// <summary>
        /// Profiling event name for executing save plugs
        /// </summary>
        private const string ProfileExecuteSavePlugsEventName = "Flex :: Execute Save Plugs";
        
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
        /// The form repository
        /// </summary>
        private readonly IFormRepository formRepository;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The task service
        /// </summary>
        private readonly ITaskService taskService;

        /// <summary>
        /// The save to database service
        /// </summary>
        private readonly ISaveToDatabaseService saveToDatabaseService;

        /// <summary>
        /// The dictionary repository
        /// </summary>
        private readonly IDictionaryRepository dictionaryRepository;

        /// <summary>
        /// The configuration manager
        /// </summary>
        private readonly IConfigurationManager configurationManager;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FlexController" /> class.
        /// </summary>
        public FlexController()
        {
            this.presentationService = DependencyResolver.Resolve<IPresentationService>();
            this.modelConverter = DependencyResolver.Resolve<IModelConverterService>();
            this.contextService = DependencyResolver.Resolve<IContextService>();
            this.userDataRepository = DependencyResolver.Resolve<IUserDataRepository>();
            this.plugsService = DependencyResolver.Resolve<IPlugsService>();
            this.flexContext = DependencyResolver.Resolve<IFlexContext>();
            this.formRepository = DependencyResolver.Resolve<IFormRepository>();
            this.logger = DependencyResolver.Resolve<ILogger>();
            this.taskService = DependencyResolver.Resolve<ITaskService>();
            this.saveToDatabaseService = DependencyResolver.Resolve<ISaveToDatabaseService>();
            this.dictionaryRepository = DependencyResolver.Resolve<IDictionaryRepository>();
            this.configurationManager = DependencyResolver.Resolve<IConfigurationManager>();
        }

        /// <summary>
        /// Controller action for GET requests
        /// </summary>
        /// <returns>Result of this action.</returns>
        public virtual ActionResult Form()
        {
            // form is not available in page editor
            if (GlassHtml.IsInEditingMode)
            {
                return this.ShowError(this.dictionaryRepository.GetText("Page Editor Message"));
            }

            Profiler.OnStart(this, ProfileGetEventName);
            
            // get the form
            var form = this.flexContext.Form;
            if (form == null)
            {
                this.logger.Warn("GET :: Form from FlexContext is null, return empty result", this);
                Profiler.OnEnd(this, ProfileGetEventName);
                return new EmptyResult();
            }

            // reset the form to not be in succeeded state anymore
            this.userDataRepository.SetFormSucceeded(form.Id, false);

            // get the current step
            var currentStep = form.GetActiveStep();
            if (currentStep == null)
            {
                this.logger.Debug("GET :: Form has no active step, return empty result", this);
                Profiler.OnEnd(this, ProfileGetEventName);
                return new EmptyResult();
            }

            // show errors
            if (!string.IsNullOrWhiteSpace(this.flexContext.ErrorMessage))
            {
                var result = this.ShowError();
                this.logger.Debug("GET :: Show error messages", this);
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

                // log
                this.logger.Debug("GET :: Clear session of current form due to a cancel request, redirect to cancel url", this);

                // redirect to correct url
                Profiler.OnEnd(this, ProfileGetEventName);
                return this.Redirect(form.CancelLink.Url);
            }

            // check if the current step may be accessed (only valid if all previous steps are done)
            // if step is not valid, redirect to the first step
            if (!this.contextService.IsStepAccessible(form, currentStep))
            {
                this.logger.Debug("GET :: Current step is not accessible, redirect to first step", this);
                Profiler.OnEnd(this, ProfileGetEventName);
                return this.Redirect(form.GetFirstStepUrl());
            }

            // revert step completion to the current step
            this.userDataRepository.RevertToStep(form.Id, currentStep.StepNumber);

            // return the view for this step
            var formViewModel = this.modelConverter.ConvertToViewModel(form);
            var formView = this.presentationService.ResolveView(this.ControllerContext, formViewModel);
            this.logger.Debug(string.Format("GET :: Everything ok, returning view '{0}'", formView), this);
            Profiler.OnEnd(this, ProfileGetEventName);
            return this.View(formView, formViewModel);
        }

        /// <summary>
        /// Controller action for POST requests.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Result of the action.</returns>
        [HttpPost]
        [ValidateFormHandler]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Form(IFormViewModel model)
        {
            // form is not available in page editor
            if (GlassHtml.IsInEditingMode)
            {
                return this.ShowError(this.dictionaryRepository.GetText("Page Editor Message"));
            }
            
            Profiler.OnStart(this, ProfilePostEventName);
            
            // get the current form
            var form = this.flexContext.Form;
            if (form == null)
            {
                this.logger.Debug("POST :: Form from FlexContext is null, return empty result", this);
                return new EmptyResult();
            }

            // show the successed message if form is is successed state
            if (this.userDataRepository.IsFormSucceeded(form.Id))
            {
                this.logger.Debug("POST :: Form has been succeeded, show success message", this);
                Profiler.OnEnd(this, ProfilePostEventName);
                return this.ShowSuccessMessage(form.SuccessMessage);
            }

            // get the current step
            var currentStep = form.GetActiveStep();
            if (currentStep == null)
            {
                this.logger.Debug("POST :: Form has no active step, return empty result", this);
                return new EmptyResult();
            }

            // check if the current step may be accessed (only valid if all previous steps are done)
            // if step is not valid, redirect to the first step
            if (!this.contextService.IsStepAccessible(form, currentStep))
            {
                this.logger.Debug("POST :: Current step is not accessible, redirect to first step", this);
                Profiler.OnEnd(this, ProfilePostEventName);
                return this.Redirect(form.GetFirstStepUrl());
            }

            // return view if we have any validation errors
            var formView = this.presentationService.ResolveView(this.ControllerContext, model);
            if (!ModelState.IsValid)
            {
                this.logger.Debug(string.Format("POST :: We have validation errors, returning view '{0}'", formView), this);
                Profiler.OnEnd(this, ProfilePostEventName);
                return this.View(formView, model);
            }

            // store the values in the session redirect to next step if we have a next step
            this.contextService.StoreFormValues(model);
            var nextStepUrl = currentStep.GetNextStepUrl();
            if (!string.IsNullOrWhiteSpace(nextStepUrl))
            {
                this.logger.Debug("POST :: Step is ok, storing values into session and redirect to next step", this);
                this.userDataRepository.CompleteStep(form.Id, currentStep.StepNumber);
                Profiler.OnEnd(this, ProfilePostEventName);
                return this.Redirect(nextStepUrl);
            }

            // repopulate the values so we also have the actual values within the current form object
            this.contextService.PopulateFormValues(this.flexContext.Form);

            // execute save plugs
            Profiler.OnStart(this, ProfileExecuteSavePlugsEventName);
            this.logger.Debug("POST :: Execute save plugs", this);
            this.plugsService.ExecuteSavePlugs(this.flexContext);
            Profiler.OnEnd(this, ProfileExecuteSavePlugsEventName);

            // show errors
            if (!string.IsNullOrWhiteSpace(this.flexContext.ErrorMessage))
            {
                this.logger.Debug("POST :: Show error messages", this);
                var result = this.ShowError();
                Profiler.OnEnd(this, ProfilePostEventName);
                return result;
            }

            // clear session values
            this.userDataRepository.ClearForm(form.Id);

            // redirect to the sucess page
            if (form.SuccessRedirect != null && !string.IsNullOrWhiteSpace(form.SuccessRedirect.Url))
            {
                this.logger.Debug("POST :: Form has been finished, return to success page", this);
                Profiler.OnEnd(this, ProfilePostEventName);
                return this.Redirect(form.SuccessRedirect.Url);
            }

            // show the success message
            this.logger.Debug("POST :: Form has been finished, show success message", this);
            this.userDataRepository.SetFormSucceeded(form.Id, true);
            Profiler.OnEnd(this, ProfilePostEventName);
            return this.ShowSuccessMessage(form.SuccessMessage);
        }

        /// <summary>
        /// Ajaxes the validator.
        /// </summary>
        /// <param name="validator">The validator.</param>
        /// <returns>Boolean value as json result</returns>
        public virtual ActionResult AjaxValidator(Guid validator)
        {
            // get the validator
            var validatorItem = this.formRepository.LoadItem<IValidator>(validator) as AjaxValidator;
            if (validatorItem == null)
            {
                this.logger.Warn(string.Format("Ajax validator with guid '{0}' not found", validator), this);
                return this.Json(false, JsonRequestBehavior.AllowGet);
            }

            // get the key of the value
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
        /// Get data for the auto complete field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>Json array with data to display</returns>
        [HttpPost]
        public virtual ActionResult AutoCompleteField(Guid field)
        {
            // get the field
            var fieldItem = this.formRepository.LoadItem<AutoCompleteField>(field);
            if (fieldItem == null)
            {
                this.logger.Warn(string.Format("Auto complete field with guid '{0}' not found", field), this);
                return this.Json(false, JsonRequestBehavior.DenyGet);
            }

            // get the key of the value
            var key = Request.Form.AllKeys.FirstOrDefault(x => x.EndsWith("Value"));
            if (string.IsNullOrWhiteSpace(key))
            {
                this.logger.Warn(string.Format("No valid value provided to get data for auto complete field '{0}'", field), this);
                return this.Json(false, JsonRequestBehavior.DenyGet);
            }

            // get the value
            var value = this.Request.Form[key].ToLowerInvariant();

            // filter the list
            var result = fieldItem.Items.Where(entry => entry.ToLowerInvariant().StartsWith(value));

            // return the list with proposed values
            return this.Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// Removes the uploaded file from the user data storage.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="field">The field.</param>
        /// <returns>Result for handling within the frontend</returns>
        public virtual ActionResult RemoveUploadedFile(string form, string field)
        {
            if (this.userDataRepository.IsFieldStored(form, field))
            {
                this.userDataRepository.SetValue(form, field, null);
            }

            return this.Content(true.ToString());
        }

        /// <summary>
        /// Executes the plugs asynchronous which are stored within the database.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="hash">The hash.</param>
        /// <returns>true/false wheater the result was ok or there was an error</returns>
        public virtual ActionResult ExecutePlugs(string timestamp, string hash)
        {
            this.logger.Debug("Received request to execute save plugs", this);
            
            // check the hash
            if (!SecurityUtil.VerifyMd5Hash(MD5.Create(), timestamp, hash))
            {
                this.logger.Warn("Invalid request/hash for executing plugs", this);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // check if async execution is allowed
            var isAsyncExecutionAllowed = this.configurationManager.Get<GlobalConfiguration>(c => c.IsAsyncExecutionAllowed);
            if (!isAsyncExecutionAllowed)
            {
                this.logger.Warn("Invalid request due to disabled async plugs execution", this);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                // execute all plugs
                this.taskService.ExecuteAll(this.flexContext.SiteContext);
            }
            catch (Exception exception)
            {
                this.logger.Error("Error while asynchronous executing plugs", this, exception);
                return this.Content(false.ToString());
            }
            
            return this.Content(true.ToString());
        }

        /// <summary>
        /// Get the exported Excel file from the database.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="hash">The hash.</param>
        /// <returns>
        /// The file from the temp folder.
        /// </returns>
        public virtual ActionResult DatabasePlugExport(Guid formId, string fileName, string hash)
        {
            // check permission
            if (!this.saveToDatabaseService.HasExportPermissions(formId))
            {
                this.logger.Warn(string.Format("Try for exporting form '{0}' with insufficient permissions", formId), this);
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            // verify hash
            if (!SecurityUtil.VerifyMd5Hash(MD5.Create(), string.Join("_", formId, fileName), hash))
            {
                this.logger.Warn(string.Format("Try for exporting form '{0}' with invalid hash", formId), this);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            // get the correct path to the temp folder
            var filePath = Path.Combine(MainUtil.MapPath(Settings.TempFolderPath), fileName);
            if (!System.IO.File.Exists(filePath))
            {
                this.logger.Warn(string.Format("Try for exporting form '{0}', but generated file '{1}' is not available anymore", formId, filePath), this);
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            // load the form to get the filename
            var form = this.contextService.LoadForm(formId.ToString());

            // get the file, delete it and return the stream
            var data = this.GetFileData(filePath);
            System.IO.File.Delete(filePath);
            var title = string.IsNullOrWhiteSpace(form.Title) ? this.dictionaryRepository.GetText("Export Fallback Filename") : form.Title;
            return this.File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Format("{0}.xlsx", title));
        }

        /// <summary>
        /// Shows the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        /// View for showing errors
        /// </returns>
        private ActionResult ShowError(string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = this.flexContext.ErrorMessage;
            }

            var model = new ErrorViewModel { Message = message };
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

        /// <summary>
        /// Gets the file data.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>File data of specific file.</returns>
        private byte[] GetFileData(string path)
        {
            using (var stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read))
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}