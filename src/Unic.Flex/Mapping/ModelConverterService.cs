namespace Unic.Flex.Mapping
{
    using AutoMapper;
    using Castle.DynamicProxy;
    using Sitecore.Diagnostics;
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using Unic.Flex.Context;
    using Unic.Flex.Globalization;
    using Unic.Flex.Model.DomainModel.Fields;
    using Unic.Flex.Model.DomainModel.Fields.InputFields;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Sections;
    using Unic.Flex.Model.DomainModel.Validators;
    using Unic.Flex.Model.Validation;
    using Unic.Flex.Model.ViewModel.Fields;
    using Unic.Flex.Model.ViewModel.Fields.InputFields;
    using Unic.Flex.Model.ViewModel.Forms;
    using Unic.Flex.Model.ViewModel.Sections;
    using Unic.Flex.Model.ViewModel.Steps;

    public class ModelConverterService : IModelConverterService
    {
        /// <summary>
        /// Cache for view model types
        /// </summary>
        private readonly ConcurrentDictionary<string, Type> viewModelTypeCache;

        private readonly IDictionaryRepository dictionaryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelConverterService"/> class.
        /// </summary>
        public ModelConverterService(IDictionaryRepository dictionaryRepository)
        {
            this.viewModelTypeCache = new ConcurrentDictionary<string, Type>();
            this.dictionaryRepository = dictionaryRepository;
        }
        
        public FormViewModel ConvertToViewModel(Form form)
        {
            Assert.ArgumentNotNull(form, "form");

            // todo: add caching (don't forget to always add the field values also when the viewmodel comes from the cache)

            // todo: add interfaces for all properties needed in minimum to map here
            
            // get the current active step
            var activeStep = form.GetActiveStep();
            if (activeStep == null) throw new Exception("No step is currently active or no step was found");

            // map the form to it's view model
            var formViewModel = this.GetViewModel<FormViewModel>(form);
            Mapper.DynamicMap(form, formViewModel, form.GetType(), formViewModel.GetType());

            // map the current step to it's view model
            var stepViewModel = this.GetViewModel<StepBaseViewModel>(activeStep);
            Mapper.DynamicMap(activeStep, stepViewModel, activeStep.GetType(), stepViewModel.GetType());
            stepViewModel.Sections.Clear();

            // add multistep properties
            var multiStepViewModel = stepViewModel as MultiStepViewModel;
            if (multiStepViewModel != null)
            {
                multiStepViewModel.NextStepUrl = activeStep.GetNextStepUrl();
                multiStepViewModel.PreviousStepUrl = activeStep.GetPreviousStepUrl();
                stepViewModel = multiStepViewModel;
            }
    
            // iterate through sections and add them
            foreach (var skinySection in activeStep.Sections)
            {
                // get the real section, because a reusable section just contains a section
                var reusableSection = skinySection as ReusableSection;
                var section = (reusableSection == null ? skinySection : reusableSection.Section) as StandardSection;

                // map the current section to it's view model
                var sectionViewModel = this.GetViewModel<StandardSectionViewModel>(section);
                Mapper.DynamicMap(section, sectionViewModel, section.GetType(), sectionViewModel.GetType());
                sectionViewModel.Fields.Clear();

                // iterate through the fields and add them
                foreach (var field in section.Fields)
                {
                    var fieldViewModel = this.GetViewModel<IFieldViewModel>(field);
                    Mapper.DynamicMap(field, fieldViewModel, field.GetType(), fieldViewModel.GetType());
                    var validatableObject = fieldViewModel as IValidatableObject;
                    if (validatableObject == null) continue;

                    // add required validator
                    if (field.IsRequired)
                    {
                        validatableObject.AddValidator(new RequiredValidator { ValidationMessage = field.ValidationMessage });
                    }

                    // add all other validators
                    foreach (var validator in field.Validators)
                    {
                        // todo: it should be possible to create placeholder within the form message in a property-name style -> #MinValue# should be replaced with property "MinValue" of the validator etc.
                        validatableObject.AddValidator(validator);
                    }

                    // add default validators of the field
                    foreach (var validator in field.DefaultValidators)
                    {
                        validator.ValidationMessage = this.dictionaryRepository.GetText(validator.ValidationMessage);
                        validatableObject.AddValidator(validator);
                    }

                    // add the field to the section
                    sectionViewModel.Fields.Add(fieldViewModel);
                }

                // add the section to the step
                stepViewModel.Sections.Add(sectionViewModel);
            }

            // add the current step to the form and return
            formViewModel.Step = stepViewModel;
            return formViewModel;
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

            return default(T);
        }

        /// <summary>
        /// Resolves the type of the view model.
        /// </summary>
        /// <param name="domainModel">The domain model.</param>
        /// <returns>Type of the view model class.</returns>
        protected virtual Type ResolveViewModelType(object domainModel)
        {
            var domainModelFullName = domainModel.GetType().FullName;
            if (this.viewModelTypeCache.ContainsKey(domainModelFullName))
            {
                return this.viewModelTypeCache[domainModelFullName];
            }

            var viewModelType = this.ResolveViewModelTypeFromAssembly(domainModel);
            this.viewModelTypeCache.TryAdd(domainModelFullName, viewModelType);
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
    }
}