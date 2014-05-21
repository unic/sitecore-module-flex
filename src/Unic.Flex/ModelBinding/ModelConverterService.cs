﻿namespace Unic.Flex.ModelBinding
{
    using System.IO;
    using System.Linq;
    using Sitecore.Diagnostics;
    using Sitecore.Globalization;
    using Sitecore.Shell.Framework.Commands.Favorites;
    using Unic.Flex.Globalization;
    using Unic.Flex.Model.Fields;
    using Unic.Flex.Model.Forms;
    using Unic.Flex.Model.Sections;
    using Unic.Flex.Model.Steps;
    using Unic.Flex.Model.Validators;

    public class ModelConverterService : IModelConverterService
    {
        private readonly IDictionaryRepository dictionaryRepository;

        public ModelConverterService(IDictionaryRepository dictionaryRepository)
        {
            this.dictionaryRepository = dictionaryRepository;
        }
        
        public FormViewModel ConvertToViewModel(Form form)
        {
            Assert.ArgumentNotNull(form, "form");
            
            var activeStep = form.GetActiveStep();
            if (activeStep == null) throw new InvalidDataException("No step is currently active or no step was found");

            var step = new StepViewModel(activeStep) { ViewName = activeStep.ViewName };
            if (activeStep is StandardStep)
            {
                var currentStep = activeStep as StandardStep;
                if (currentStep.Sections == null || !currentStep.Sections.Any()) throw new InvalidDataException("Active step does not contain any sections");

                foreach (var section in currentStep.Sections)
                {
                    var realSection = (section is ReusableSection
                        ? (section as ReusableSection).Section
                        : section) as StandardSection;

                    var sectionViewModel = new SectionViewModel(realSection) { ViewName = realSection.ViewName };
                    sectionViewModel.DisableFieldset = realSection.DisableFieldset;
                    sectionViewModel.Title = realSection.Title;

                    foreach (var field in realSection.Fields)
                    {
                        var fieldViewModel = new FieldViewModel(field);
                        fieldViewModel.Key = field.ItemId.ToString();
                        fieldViewModel.Label = field.Label;
                        fieldViewModel.ViewName = field.ViewName;
                        fieldViewModel.Value = field.Value as string;

                        // add required validator
                        if (field.IsRequired)
                        {
                            var validator = new RequiredValidator { ValidationMessage = field.ValidationMessage };
                            if (string.IsNullOrWhiteSpace(validator.ValidationMessage))
                            {
                                // todo: add a custom glass mapper attribute to create a dictionary fallback
                                validator.ValidationMessage = this.dictionaryRepository.GetText("Field is required");
                            }

                            fieldViewModel.AddValidator(validator);
                        }

                        // add all other validators
                        foreach (var validator in field.Validators)
                        {
                            fieldViewModel.AddValidator(validator);
                        }

                        sectionViewModel.Fields.Add(fieldViewModel);
                    }

                    step.Sections.Add(sectionViewModel);
                }
            }

            return new FormViewModel
            {
                Title = form.Title,
                Introduction = form.Introduction,
                Step = step
            };
        }
    }
}