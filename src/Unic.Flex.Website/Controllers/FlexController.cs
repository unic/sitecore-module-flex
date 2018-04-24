namespace Unic.Flex.Website.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography;
    using System.Web;
    using System.Web.Mvc;
    using Configuration.Core;
    using Core.Attributes;
    using Core.Context;
    using Core.Database;
    using Core.Globalization;
    using Core.Logging;
    using Core.Mapping;
    using Core.Plugs;
    using Core.Presentation;
    using Core.Utilities;
    using Glass.Mapper.Sc;
    using Implementation.Database;
    using Implementation.Fields.InputFields;
    using Implementation.Plugs.SavePlugs;
    using Implementation.Services;
    using Implementation.Validators;
    using Model.Configuration;
    using Model.DataProviders;
    using Model.Fields;
    using Model.Fields.ListFields;
    using Model.Forms;
    using Model.Steps;
    using Model.Validators;
    using Model.ViewModels;
    using Models.CascadingFields;
    using Newtonsoft.Json;
    using Profiling;
    using Sitecore;
    using Constants = Implementation.Definitions.Constants;
    using Convert = System.Convert;
    using DependencyResolver = Core.DependencyInjection.DependencyResolver;
    using Settings = Sitecore.Configuration.Settings;

    public class FlexController : Controller
    {
        private const string ProfileGetEventName = "Flex :: GET Controller Action";
        private const string ProfilePostEventName = "Flex :: POST Controller Action";
        private const string ProfileExecuteSavePlugsEventName = "Flex :: Execute Save Plugs";

        private readonly IPresentationService presentationService;
        private readonly IContextService contextService;
        private readonly IUserDataRepository userDataRepository;
        private readonly IPlugsService plugsService;
        private readonly IFlexContext flexContext;
        private readonly IFormRepository formRepository;
        private readonly ILogger logger;
        private readonly ITaskService taskService;
        private readonly ISaveToDatabaseService saveToDatabaseService;
        private readonly IDictionaryRepository dictionaryRepository;
        private readonly IConfigurationManager configurationManager;
        private readonly IViewMapper viewMapper;
        private readonly IDoubleOptinService doubleOptinService;
        private readonly IDoubleOptinLinkService doubleOptinLinkService;
        private readonly IUnitOfWork unitOfWork;

        public FlexController()
        {
            this.presentationService = DependencyResolver.Resolve<IPresentationService>();
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
            this.viewMapper = DependencyResolver.Resolve<IViewMapper>();
            this.doubleOptinService = DependencyResolver.Resolve<IDoubleOptinService>();
            this.doubleOptinLinkService = DependencyResolver.Resolve<IDoubleOptinLinkService>();
            this.unitOfWork = DependencyResolver.Resolve<IUnitOfWork>();
        }

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

            //check if there are query parameters
            if (Context.Request.QueryString.AllKeys.Contains(Constants.ScActionQueryKey) && Context.Request.QueryString[Constants.ScActionQueryKey] == Constants.OptinQueryKey)
            {
                var optInFormId = Uri.UnescapeDataString(Context.Request.QueryString[Constants.OptInFormIdKey]);
                var optInRecordId = Uri.UnescapeDataString(Context.Request.QueryString[Constants.OptInRecordIdKey]);
                var optInHash = Uri.UnescapeDataString(Context.Request.QueryString[Constants.OptInHashKey]);
                
                var doubleOptinSavePlug = form.SavePlugs.OfType<DoubleOptinSavePlug>().FirstOrDefault();

                if (doubleOptinSavePlug != null)
                {
                    var fields = this.unitOfWork.SessionRepository.GetById(Convert.ToInt32(optInRecordId)).Fields;
                    var email = fields.FirstOrDefault(x => x.ItemId == doubleOptinSavePlug.To.ItemId)?.Value;
                    if (this.doubleOptinLinkService.ValidateConfirmationLink(optInFormId, optInRecordId, email, optInHash))
                    {
                        this.doubleOptinService.ExecuteSubSavePlugs(doubleOptinSavePlug, flexContext, optInRecordId);

                        if (!string.IsNullOrWhiteSpace(this.flexContext.ErrorMessage))
                        {
                            Profiler.OnEnd(this, ProfileGetEventName);
                            return this.ShowError();
                        }

                        Profiler.OnEnd(this, ProfileGetEventName);
                        return this.ShowSuccessMessage(doubleOptinSavePlug.ConfirmMessage);
                    }
                }
            }

            // reset the form to not be in succeeded state anymore
            this.userDataRepository.SetFormSucceeded(form.Id, false);

            // get the current step
            var currentStep = form.ActiveStep;
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
            if (Request.QueryString[Core.Definitions.Constants.CancelQueryStringKey] == Core.Definitions.Constants.CancelQueryStringValue
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

            // map the active step
            this.viewMapper.Map(this.flexContext);

            // return the view for this step
            var formView = this.presentationService.ResolveView(this.ControllerContext, form);
            this.logger.Debug(string.Format("GET :: Everything ok, returning view '{0}'", formView), this);
            Profiler.OnEnd(this, ProfileGetEventName);
            return this.View(formView, form);
        }

        /// <summary>
        /// Controller action for POST requests.
        /// </summary>
        /// <param name="formModel">The model.</param>
        /// <returns>Result of the action.</returns>
        [HttpPost]
        [ValidateFormHandler]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Form(IForm formModel)
        {
            // Note (dsz, 14.11.2017): Do not rename the formModel parameter to 'model' as this will cause clashes
            // with nitronet in DefaultModelBinder when searching for a matching key in the value providers.
            // 'Model', along with 'template' and 'data' seems to be added by Nitro.Net to the RouteDataValueProvider
            // and ChildActionValueProvider.

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
            var currentStep = form.ActiveStep;
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
            var formView = this.presentationService.ResolveView(this.ControllerContext, formModel);
            if (!this.ModelState.IsValid)
            {
                this.logger.Debug(string.Format("POST :: We have validation errors, returning view '{0}'", formView), this);
                Profiler.OnEnd(this, ProfilePostEventName);
                return this.View(formView, formModel);
            }

            // store the values in the session redirect to next step if we have a next step
            this.contextService.StoreFormValues(formModel);
            var multistep = currentStep as MultiStep;
            if (multistep != null && !string.IsNullOrWhiteSpace(multistep.NextStepUrl))
            {
                this.logger.Debug("POST :: Step is ok, storing values into session and redirect to next step", this);
                this.userDataRepository.CompleteStep(form.Id, currentStep.StepNumber);
                Profiler.OnEnd(this, ProfilePostEventName);
                return this.Redirect(multistep.NextStepUrl);
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
            AjaxValidator validatorItem;
            using (new VersionCountDisabler())
            {
                validatorItem = this.formRepository.LoadItem<IValidator>(validator) as AjaxValidator;
            }

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
            var key = this.Request.Form.AllKeys.FirstOrDefault(x => x.EndsWith("Value"));
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
        /// Get the new data for a cascading field. This actually only works for listfields with string as value
        /// an ListItem as data provider type.
        /// </summary>
        /// <param name="field">The field we want to retrieve data.</param>
        /// <returns>
        /// Json array with data to display
        /// </returns>
        public virtual ActionResult CascadingField(Guid field)
        {
            // get the field
            var fieldModel = this.formRepository.LoadItem<IField>(field) as ListField<string, ListItem>;
            if (fieldModel == null)
            {
                this.logger.Warn(string.Format("Cascading field with id '{0}' was not found", field), this);
                return this.Content("error");
            }

            // check if the field is a dependent field
            if (!fieldModel.IsCascadingField)
            {
                this.logger.Warn(string.Format("Field with id '{0}' is not configured as cascading field", field), this);
                return this.Content("error");
            }

            // set value property of the field models which are dependent
            var collection = this.Request.QueryString;
            var keys = collection.AllKeys.Where(k => k.EndsWith(".Value")).OrderByDescending(k => k).ToList();
            var counter = 0;
            var dependentField = fieldModel.DependentField as ListField<string, ListItem>;
            while (dependentField != null)
            {
                if (counter >= keys.Count) break;

                dependentField.SetValue(collection[keys[counter++]]);
                dependentField = dependentField.DependentField as ListField<string, ListItem>;
            }

            // create the data
            var data = new CascadingField { Options = fieldModel.Items.Select(i => new CascadingOption { Text = i.Text, Value = i.Value, Selected = i.Selected }).ToList() };
            if (data.Options != null && data.Options.Any() && !data.Options.Any(o => o.Selected) && !string.IsNullOrWhiteSpace(data.Options.First().Value))
            {
                data.Options.First().Selected = true;
            }

            // create the response
            this.Response.ContentType = "application/json";
            return this.Content(JsonConvert.SerializeObject(data));
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