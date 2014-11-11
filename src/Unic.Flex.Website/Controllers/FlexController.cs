namespace Unic.Flex.Website.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography;
    using System.Web.Mvc;
    using Glass.Mapper.Sc;
    using Sitecore;
    using Unic.Configuration;
    using Unic.Flex.Attributes;
    using Unic.Flex.Context;
    using Unic.Flex.Globalization;
    using Unic.Flex.Implementation.Database;
    using Unic.Flex.Implementation.Fields.InputFields;
    using Unic.Flex.Logging;
    using Unic.Flex.Mapping;
    using Unic.Flex.Model.Configuration;
    using Unic.Flex.Model.Validation;
    using Unic.Flex.Model.ViewModel.Components;
    using Unic.Flex.Model.ViewModel.Forms;
    using Unic.Flex.Plugs;
    using Unic.Flex.Presentation;
    using Unic.Flex.Utilities;
    using Constants = Unic.Flex.Definitions.Constants;
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

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="FlexController" /> class.
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
        /// <param name="taskService">The task service.</param>
        /// <param name="saveToDatabaseService">The save to database service.</param>
        /// <param name="dictionaryRepository">The dictionary repository.</param>
        /// <param name="configurationManager">The configuration manager.</param>
        public FlexController(IPresentationService presentationService, IModelConverterService modelConverter, IContextService contextService, IUserDataRepository userDataRepository, IPlugsService plugsService, IFlexContext flexContext, IUrlService urlService, IFormRepository formRepository, ILogger logger, ITaskService taskService, ISaveToDatabaseService saveToDatabaseService, IDictionaryRepository dictionaryRepository, IConfigurationManager configurationManager)
        {
            //// todo: check if all service/repository classes have virtual methods
            
            this.presentationService = presentationService;
            this.modelConverter = modelConverter;
            this.contextService = contextService;
            this.userDataRepository = userDataRepository;
            this.plugsService = plugsService;
            this.flexContext = flexContext;
            this.urlService = urlService;
            this.formRepository = formRepository;
            this.logger = logger;
            this.taskService = taskService;
            this.saveToDatabaseService = saveToDatabaseService;
            this.dictionaryRepository = dictionaryRepository;
            this.configurationManager = configurationManager;
        }

        /// <summary>
        /// Controller action for GET requests
        /// </summary>
        /// <returns>Result of this action.</returns>
        public ActionResult Form()
        {
            // form is not available in page editor
            if (GlassHtml.IsInEditingMode)
            {
                return this.ShowError(this.dictionaryRepository.GetText("Page Editor Message"));
            }

            Profiler.OnStart(this, ProfileGetEventName);
            
            // get the form
            var form = this.flexContext.Form;
            if (form == null) return new EmptyResult();

            // get the current step
            var currentStep = form.GetActiveStep();
            if (currentStep == null) return new EmptyResult();

            // show errors
            if (!string.IsNullOrWhiteSpace(this.flexContext.ErrorMessage))
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
        [ValidateFormHandler]
        [ValidateAntiForgeryToken]
        public ActionResult Form(IFormViewModel model)
        {
            // form is not available in page editor
            if (GlassHtml.IsInEditingMode)
            {
                return this.ShowError(this.dictionaryRepository.GetText("Page Editor Message"));
            }
            
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
            this.contextService.StoreFormValues(model);
            var nextStepUrl = currentStep.GetNextStepUrl();
            if (!string.IsNullOrWhiteSpace(nextStepUrl))
            {
                this.userDataRepository.CompleteStep(form.Id, currentStep.StepNumber);
                Profiler.OnEnd(this, ProfilePostEventName);
                return this.Redirect(nextStepUrl);
            }

            // repopulate the values so we also have the actual values within the current form object
            this.contextService.PopulateFormValues(this.flexContext.Form);

            // execute save plugs
            Profiler.OnStart(this, ProfileExecuteSavePlugsEventName);
            this.plugsService.ExecuteSavePlugs(this.flexContext);
            Profiler.OnEnd(this, ProfileExecuteSavePlugsEventName);

            // show errors
            if (!string.IsNullOrWhiteSpace(this.flexContext.ErrorMessage))
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
        public ActionResult AutoCompleteField(Guid field)
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
        public ActionResult RemoveUploadedFile(string form, string field)
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
        public ActionResult ExecutePlugs(string timestamp, string hash)
        {
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
        public ActionResult DatabasePlugExport(Guid formId, string fileName, string hash)
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
            return this.File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Format("{0}.xlsx", form.Title));
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