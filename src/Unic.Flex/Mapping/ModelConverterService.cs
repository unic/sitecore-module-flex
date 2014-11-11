namespace Unic.Flex.Mapping
{
    using AutoMapper;
    using Castle.DynamicProxy;
    using Sitecore.Diagnostics;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Unic.Configuration;
    using Unic.Flex.Context;
    using Unic.Flex.Definitions;
    using Unic.Flex.Globalization;
    using Unic.Flex.Model.Configuration;
    using Unic.Flex.Model.DomainModel.Components;
    using Unic.Flex.Model.DomainModel.Fields.ListFields;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Steps;
    using Unic.Flex.Model.Validation;
    using Unic.Flex.Model.ViewModel.Components;
    using Unic.Flex.Model.ViewModel.Fields;
    using Unic.Flex.Model.ViewModel.Fields.InputFields;
    using Unic.Flex.Model.ViewModel.Forms;
    using Unic.Flex.Model.ViewModel.Sections;
    using Unic.Flex.Model.ViewModel.Steps;
    using Unic.Flex.Utilities;
    using Profiler = Unic.Profiling.Profiler;

    /// <summary>
    /// Service to convert between domain and view model.
    /// </summary>
    public class ModelConverterService : IModelConverterService
    {
        /// <summary>
        /// The type lock
        /// </summary>
        private static readonly object TypeLock = new object();
        
        /// <summary>
        /// Cache for view model types
        /// </summary>
        private readonly IDictionary<string, Type> viewModelTypeCache;

        /// <summary>
        /// The dictionary repository
        /// </summary>
        private readonly IDictionaryRepository dictionaryRepository;

        /// <summary>
        /// The configuration manager
        /// </summary>
        private readonly IConfigurationManager configurationManager;

        /// <summary>
        /// The URL service
        /// </summary>
        private readonly IUrlService urlService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelConverterService" /> class.
        /// </summary>
        /// <param name="dictionaryRepository">The dictionary repository.</param>
        /// <param name="urlService">The URL service.</param>
        /// <param name="configurationManager">The configuration manager.</param>
        public ModelConverterService(IDictionaryRepository dictionaryRepository, IUrlService urlService, IConfigurationManager configurationManager)
        {
            this.viewModelTypeCache = new Dictionary<string, Type>();
            this.dictionaryRepository = dictionaryRepository;
            this.urlService = urlService;
            this.configurationManager = configurationManager;
        }

        /// <summary>
        /// Converts the domain model to view model.
        /// </summary>
        /// <param name="form">The form domain model.</param>
        /// <returns>
        /// The view model
        /// </returns>
        /// <exception cref="System.Exception">No step is currently active or no step was found</exception>
        public IFormViewModel ConvertToViewModel(Form form)
        {
            Assert.ArgumentNotNull(form, "form");

            // get the current active step
            var activeStep = form.GetActiveStep();
            if (activeStep == null) throw new Exception("No step is currently active or no step was found");

            // convert the step
            Profiler.OnStart(this, "Flex :: Convert DomainModel to ViewModel");
            var viewModel = this.Convert(form, activeStep);
            Profiler.OnEnd(this, "Flex :: Convert DomainModel to ViewModel");
            return viewModel;
        }

        /// <summary>
        /// Gets a view model instance base on the domain model.
        /// The view model must have the same class name as the domain model with the postfix "ViewModel".
        /// Both classes have to be in the same assembly.
        /// </summary>
        /// <typeparam name="T">Interface type of the desired view model</typeparam>
        /// <param name="domainModel">The domain model.</param>
        /// <returns>
        /// A newly created view model, if class was found.
        /// </returns>
        public virtual T GetViewModel<T>(object domainModel)
        {
            Assert.ArgumentNotNull(domainModel, "domainModel");
            var viewModelType = this.ResolveViewModelType(domainModel);
            if (viewModelType != null && typeof(T).IsAssignableFrom(viewModelType))
            {
                return (T)Activator.CreateInstance(viewModelType);
            }

            throw new TypeLoadException(string.Format("Could not find corresponding view model for type '{0}'", ProxyUtil.GetUnproxiedType(domainModel).FullName));
        }

        /// <summary>
        /// Converts the specified form domain model to the view model.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="step">The step.</param>
        /// <returns>View model</returns>
        protected virtual IFormViewModel Convert(Form form, StepBase step)
        {
            Assert.ArgumentNotNull(form, "form");
            Assert.ArgumentNotNull(step, "step");

            // get config
            var optionalLabelText = this.configurationManager.Get<GlobalConfiguration>(c => c.OptionalFieldsLabelText);

            // handle summary step
            var summary = step as Summary;
            if (summary != null)
            {
                step.Sections = form.Steps.SelectMany(s => s.Sections).ToList();
            }

            // map the form to it's view model
            var formViewModel = this.GetViewModel<IFormViewModel>(form);
            Mapper.Map(form, formViewModel, form.GetType(), formViewModel.GetType());

            // map the current step to it's view model
            var stepViewModel = this.GetViewModel<IStepViewModel>(step);
            Mapper.Map(step, stepViewModel, step.GetType(), stepViewModel.GetType());

            // add the navigation pane for multi steps
            var navigationPane = stepViewModel as INavigationPaneViewModel;
            if (navigationPane != null && navigationPane.ShowNavigationPane)
            {
                navigationPane.NavigationPane = this.GetNavigationPane(form, step.StepNumber);
            }

            // get the text for the canlce link
            if (form.CancelLink != null && !string.IsNullOrWhiteSpace(form.CancelLink.Url))
            {
                stepViewModel.CancelUrl = this.urlService.AddQueryStringToCurrentUrl(
                    Constants.CancelQueryStringKey,
                    Constants.CancelQueryStringValue);

                stepViewModel.CancelText = !string.IsNullOrWhiteSpace(form.CancelLink.Text)
                                 ? form.CancelLink.Text
                                 : this.dictionaryRepository.GetText("Cancel Form");
            }

            // iterate through sections and add them
            foreach (var section in step.Sections)
            {
                // map the current section to it's view model
                var sectionViewModel = this.GetViewModel<ISectionViewModel>(section);
                Mapper.Map(section, sectionViewModel, section.GetType(), sectionViewModel.GetType());

                // handle field dependency
                if (section.DependentField != null)
                {
                    sectionViewModel.DependentFrom = section.DependentField.ReusableComponent != null
                                                           ? section.DependentField.ReusableComponent.Id
                                                           : section.DependentField.Id;
                }

                // add tooltip
                sectionViewModel.Tooltip = new TooltipViewModel { TooltipTitle = section.TooltipTitle, TooltipText = section.TooltipText };

                // iterate through the fields and add them
                foreach (var field in section.Fields)
                {
                    // skip if on summary the field should not be shown
                    if (summary != null && !field.ShowInSummary) continue;
                    
                    // map the field
                    var fieldViewModel = this.GetViewModel<IFieldViewModel>(field);
                    Mapper.DynamicMap(field, fieldViewModel, field.GetType(), fieldViewModel.GetType());
                    var validatableObject = fieldViewModel as IValidatableObject;
                    if (validatableObject == null) continue;

                    // handle field dependency
                    if (field.DependentField != null)
                    {
                        fieldViewModel.DependentFrom = field.DependentField.ReusableComponent != null
                                                           ? field.DependentField.ReusableComponent.Id
                                                           : field.DependentField.Id;
                    }

                    // add required validator
                    if (field.IsRequired)
                    {
                        if (TypeHelper.IsSubclassOfRawGeneric(typeof(MulticheckListField<>), field.GetType()))
                        {
                            validatableObject.AddValidator(new MulticheckRequired { ValidationMessage = field.ValidationMessage });
                        }
                        else
                        {
                            validatableObject.AddValidator(new RequiredValidator { ValidationMessage = field.ValidationMessage });    
                        }       
                    }
                    else if (!string.IsNullOrWhiteSpace(optionalLabelText))
                    {
                        fieldViewModel.LabelAddition = optionalLabelText;
                    }

                    // add all other validators
                    foreach (var validator in field.Validators.Concat(field.DefaultValidators))
                    {
                        if (string.IsNullOrWhiteSpace(validator.ValidationMessage))
                        {
                            validator.ValidationMessage = this.dictionaryRepository.GetText(validator.DefaultValidationMessageDictionaryKey);
                        }

                        validatableObject.AddValidator(validator);
                    }

                    // add css classes
                    if (field.CustomCssClass != null && !string.IsNullOrWhiteSpace(field.CustomCssClass.Value))
                    {
                        fieldViewModel.AddCssClass(field.CustomCssClass.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(field.AdditionalCssClass))
                    {
                        fieldViewModel.AddCssClass(field.AdditionalCssClass);
                    }

                    // add tooltip
                    fieldViewModel.Tooltip = new TooltipViewModel { TooltipTitle = field.TooltipTitle, TooltipText = field.TooltipText };

                    // bind properties
                    fieldViewModel.BindProperties();

                    // add the field to the section
                    sectionViewModel.Fields.Add(fieldViewModel);
                }

                // check if we have any fields
                if (!sectionViewModel.Fields.Any()) continue;

                // bind the properties
                sectionViewModel.BindProperties();

                // add the section to the step
                stepViewModel.Sections.Add(sectionViewModel);
            }

            // add the honeypot field
            if (summary == null)
            {
                this.AddHoneypotField(stepViewModel);
            }

            // add the current step to the form and return
            formViewModel.Step = stepViewModel;
            return formViewModel;
        }

        /// <summary>
        /// Resolves the type of the view model.
        /// </summary>
        /// <param name="domainModel">The domain model.</param>
        /// <returns>Type of the view model class.</returns>
        protected virtual Type ResolveViewModelType(object domainModel)
        {
            Type viewModelType;
            var domainModelFullName = domainModel.GetType().FullName;

            lock (TypeLock)
            {
                if (this.viewModelTypeCache.ContainsKey(domainModelFullName))
                {
                    return this.viewModelTypeCache[domainModelFullName];
                }

                viewModelType = this.ResolveViewModelTypeFromAssembly(domainModel);
                this.viewModelTypeCache.Add(domainModelFullName, viewModelType);
            }

            return viewModelType;
        }

        /// <summary>
        /// Resolves the view model type from the assembly. It searches for class with the same name as the domain model
        /// type with "ViewModel" at the end -> i.e. for the domain model class "TextField" it searches for "TextFieldViewModel".
        /// The search is done within the same assembly as the domain model only.
        /// </summary>
        /// <param name="domainModel">The domain model instance.</param>
        /// <returns>Type of the view model class.</returns>
        protected virtual Type ResolveViewModelTypeFromAssembly(object domainModel)
        {
            var domainModelType = ProxyUtil.GetUnproxiedType(domainModel);
            var viewModelClassName = domainModelType.Name + "ViewModel";
            return domainModelType.Assembly.GetTypes().FirstOrDefault(type => type.Name == viewModelClassName);
        }

        /// <summary>
        /// Gets the navigation pane.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="currentStep">The current step.</param>
        /// <returns>Navigation pane with navigation items</returns>
        protected virtual NavigationPaneViewModel GetNavigationPane(Form form, int currentStep)
        {
            Assert.ArgumentNotNull(form, "form");
            
            var navigationPane = new NavigationPaneViewModel();
            foreach (var step in form.Steps)
            {
                navigationPane.Items.Add(new NavigationItem
                                         {
                                             IsActive = step.StepNumber == currentStep,
                                             IsLinked = step.StepNumber < currentStep,
                                             Title = step.Title,
                                             Url = step.GetUrl()
                                         });
            }

            return navigationPane;
        }

        /// <summary>
        /// Adds the honeypot field for spam protection to the current step.
        /// </summary>
        /// <param name="stepViewModel">The step view model.</param>
        protected virtual void AddHoneypotField(IStepViewModel stepViewModel)
        {
            Assert.ArgumentNotNull(stepViewModel, "stepViewModel");

            // get the first section
            var section = stepViewModel.Sections.FirstOrDefault();
            if (section == null) return;

            // generate the field
            var honeypot = new HoneypotFieldViewModel();
            honeypot.Id = "info3";
            honeypot.Label = this.dictionaryRepository.GetText("Leave this blank if you are human");
            honeypot.BindProperties();

            // add validator
            var validator = new HoneypotValidator();
            validator.ValidationMessage = this.dictionaryRepository.GetText(validator.DefaultValidationMessageDictionaryKey);
            honeypot.AddValidator(validator);
            
            // add field
            section.Fields.Add(honeypot);
        }
    }
}