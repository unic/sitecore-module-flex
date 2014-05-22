namespace Unic.Flex.ModelBinding
{
    using System.IO;
    using System.Linq;
    using Sitecore.Diagnostics;
    using Sitecore.Globalization;
    using Sitecore.Shell.Framework.Commands.Favorites;
    using Unic.Flex.Globalization;
    using Unic.Flex.Model.DomainModel.Fields;
    using Unic.Flex.Model.DomainModel.Fields.InputFields;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Sections;
    using Unic.Flex.Model.DomainModel.Steps;
    using Unic.Flex.Model.DomainModel.Validators;
    using Unic.Flex.Model.ViewModel.Fields;
    using Unic.Flex.Model.ViewModel.Fields.InputFields;
    using Unic.Flex.Model.ViewModel.Forms;
    using Unic.Flex.Model.ViewModel.Sections;
    using Unic.Flex.Model.ViewModel.Steps;

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
                        InputFieldViewModel<string> fieldViewModel;
                        if(field is SinglelineTextField)
                        {
                            fieldViewModel = new SinglelineTextFieldViewModel();
                        }
                        else
                        {
                            fieldViewModel = new MultilineTextFieldViewModel();
                        }

                        fieldViewModel.Key = field.ItemId.ToString();
                        fieldViewModel.Label = field.Label;
                        fieldViewModel.ViewName = field.ViewName;
                        
                        // todo: this must be generic not a string
                        fieldViewModel.Value = (field as FieldBase<string>).Value as string;

                        // add required validator
                        if (field.IsRequired)
                        {
                            fieldViewModel.AddValidator(new RequiredValidator { ValidationMessage = field.ValidationMessage });
                        }

                        // add all other validators
                        foreach (var validator in field.Validators)
                        {
                            fieldViewModel.AddValidator(validator);
                        }

                        // todo: validators should handle format string -> i.e {0} in the validation message should be replaced with the field name

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