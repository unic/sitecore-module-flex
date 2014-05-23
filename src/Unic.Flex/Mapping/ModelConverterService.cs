namespace Unic.Flex.Mapping
{
    using System;
    using System.Linq;
    using AutoMapper;
    using Castle.DynamicProxy;
    using Sitecore.Diagnostics;
    using Unic.Flex.Context;
    using Unic.Flex.Model.DomainModel.Fields;
    using Unic.Flex.Model.DomainModel.Fields.InputFields;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Sections;
    using Unic.Flex.Model.DomainModel.Steps;
    using Unic.Flex.Model.DomainModel.Validators;
    using Unic.Flex.Model.ViewModel;
    using Unic.Flex.Model.ViewModel.Fields.InputFields;
    using Unic.Flex.Model.ViewModel.Forms;
    using Unic.Flex.Model.ViewModel.Sections;
    using Unic.Flex.Model.ViewModel.Steps;

    public class ModelConverterService : IModelConverterService
    {
        public FormViewModel ConvertToViewModel(Form form)
        {
            Assert.ArgumentNotNull(form, "form");

            // todo: add caching (don't forget to always add the field values also when the viewmodel comes from the cache)

            // todo: add interfaces for all properties needed in minimum to map here
            
            // get the current active step
            var activeStep = form.GetActiveStep();
            if (activeStep == null) throw new Exception("No step is currently active or no step was found");

            // map the form to it's view model
            var formViewModel = GetViewModel<FormViewModel>(form);
            Mapper.DynamicMap(form, formViewModel, form.GetType(), formViewModel.GetType());

            // map the current step to it's view model
            var stepViewModel = GetViewModel<StepBaseViewModel>(activeStep);
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
                var sectionViewModel = GetViewModel<StandardSectionViewModel>(section);
                Mapper.DynamicMap(section, sectionViewModel, section.GetType(), sectionViewModel.GetType());
                sectionViewModel.Fields.Clear();

                // iterate through the fields and add them
                foreach (var field in section.Fields)
                {
                    // todo: this must be generic
                    var fieldViewModel = GetViewModel<InputFieldViewModel<string>>(field);
                    Mapper.DynamicMap(field, fieldViewModel, field.GetType(), fieldViewModel.GetType());
                    
                    // add required validator
                    if (field.IsRequired)
                    {
                        fieldViewModel.AddValidator(new RequiredValidator { ValidationMessage = field.ValidationMessage });
                    }

                    // add all other validators
                    foreach (var validator in field.Validators)
                    {
                        // todo: validators should handle format string -> i.e {0} in the validation message should be replaced with the field name
                        fieldViewModel.AddValidator(validator);
                    }

                    // todo: remove this
                    fieldViewModel.Key = field.ItemId.ToString();

                    // todo: this must be generic not a string -> also it should not be set here becaue of the caching
                    fieldViewModel.Value = (field as FieldBase<string>).Value as string;

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
        private static T GetViewModel<T>(object domainModel)
        {
            Assert.ArgumentNotNull(domainModel, "domainModel");
            
            // todo: add caching to rewtrieving types

            var domainModelType = ProxyUtil.GetUnproxiedType(domainModel);
            var name = domainModelType.Name + "ViewModel";
            var viewModelType = domainModelType.Assembly.GetTypes().FirstOrDefault(type => type.Name == name);
            return viewModelType == null ? default(T) : (T)Activator.CreateInstance(viewModelType);
        }
    }
}