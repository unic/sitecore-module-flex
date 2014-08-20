namespace Unic.Flex.Mapping
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using AutoMapper;
    using Castle.DynamicProxy;
    using Sitecore.Diagnostics;
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using Sitecore.Exceptions;
    using Unic.Flex.Context;
    using Unic.Flex.Definitions;
    using Unic.Flex.Globalization;
    using Unic.Flex.Logging;
    using Unic.Flex.Model.DomainModel.Fields.ListFields;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Sections;
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

    public class ModelConverterService : IModelConverterService
    {
        /// <summary>
        /// Cache for view model types
        /// </summary>
        private readonly IDictionary<string, Type> viewModelTypeCache;

        private static readonly object typeLock = new object();

        private readonly IDictionaryRepository dictionaryRepository;

        private readonly IUrlService urlService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelConverterService"/> class.
        /// </summary>
        public ModelConverterService(IDictionaryRepository dictionaryRepository, IUrlService urlService)
        {
            this.viewModelTypeCache = new Dictionary<string, Type>();
            this.dictionaryRepository = dictionaryRepository;
            this.urlService = urlService;
        }

        public IFormViewModel ConvertToViewModel(Form form)
        {
            Assert.ArgumentNotNull(form, "form");

            // get the current active step
            var activeStep = form.GetActiveStep();
            if (activeStep == null) throw new Exception("No step is currently active or no step was found");

            // convert the step
            Profiler.OnStart(this, "Convert DomainModel to ViewModel");
            var viewModel = this.Convert(form, activeStep);
            Profiler.OnEnd(this, "Convert DomainModel to ViewModel");
            return viewModel;
        }

        /// <summary>
        /// Gets a new view model instance based on the corresponding domain model.
        /// </summary>
        /// <typeparam name="T">Type of the view model</typeparam>
        /// <param name="domainModel">The domain model.</param>
        /// <returns>New instance of the found view model or null</returns>
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

        protected virtual IFormViewModel Convert(Form form, StepBase step)
        {
            Assert.ArgumentNotNull(form, "form");
            Assert.ArgumentNotNull(step, "step");

            // handle summary step
            var summary = step as Summary;
            if (summary != null)
            {
                // todo: this need to be done someway different -> also we need to structure the sections by step and not only the sections
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
                // get the real section, because a reusable section just contains a section
                var reusableSection = section as ReusableSection;
                var realSection = (reusableSection == null ? section : reusableSection.Section) as StandardSection;
                if (realSection == null) continue;

                // map the current section to it's view model
                var sectionViewModel = this.GetViewModel<ISectionViewModel>(realSection);
                Mapper.Map(realSection, sectionViewModel, realSection.GetType(), sectionViewModel.GetType());

                // add tooltip
                sectionViewModel.Tooltip = new TooltipViewModel { TooltipTitle = realSection.TooltipTitle, TooltipText = realSection.TooltipText };

                // iterate through the fields and add them
                foreach (var field in realSection.Fields)
                {
                    var fieldViewModel = this.GetViewModel<IFieldViewModel>(field);
                    Mapper.DynamicMap(field, fieldViewModel, field.GetType(), fieldViewModel.GetType());
                    var validatableObject = fieldViewModel as IValidatableObject;
                    if (validatableObject == null) continue;

                    // bind properties
                    fieldViewModel.BindProperties();

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

                    // add the field to the section
                    sectionViewModel.Fields.Add(fieldViewModel);
                }

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

            lock (typeLock)
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
            honeypot.Key = "info3";
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