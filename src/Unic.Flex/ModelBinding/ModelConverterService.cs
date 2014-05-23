namespace Unic.Flex.ModelBinding
{
    using Sitecore.Diagnostics;
    using System;
    using Unic.Flex.Context;
    using Unic.Flex.Model.DomainModel.Fields;
    using Unic.Flex.Model.DomainModel.Fields.InputFields;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Sections;
    using Unic.Flex.Model.DomainModel.Steps;
    using Unic.Flex.Model.DomainModel.Validators;
    using Unic.Flex.Model.ViewModel.Fields.InputFields;
    using Unic.Flex.Model.ViewModel.Forms;
    using Unic.Flex.Model.ViewModel.Sections;
    using Unic.Flex.Model.ViewModel.Steps;

    public class ModelConverterService : IModelConverterService
    {
        public FormViewModel ConvertToViewModel(Form form)
        {
            Assert.ArgumentNotNull(form, "form");
            
            var activeStep = form.GetActiveStep();
            if (activeStep == null) throw new Exception("No step is currently active or no step was found");

            StepBaseViewModel step = null;
            if (activeStep is Summary)
            {
                step = new SummaryViewModel { ViewName = activeStep.ViewName };
            }
            else
            {
                if (activeStep is SingleStep)
                {
                    step = new SingleStepViewModel { ViewName = activeStep.ViewName };
                }
                else
                {
                    step = new MultiStepViewModel { ViewName = activeStep.ViewName };
                }
                
                var currentStep = activeStep as StandardStep;
                foreach (var section in currentStep.Sections)
                {
                    var realSection =
                        (section is ReusableSection ? (section as ReusableSection).Section : section) as
                        StandardSection;

                    var sectionViewModel = new StandardSectionViewModel { ViewName = realSection.ViewName };
                    sectionViewModel.DisableFieldset = realSection.DisableFieldset;
                    sectionViewModel.Title = realSection.Title;

                    foreach (var field in realSection.Fields)
                    {
                        InputFieldViewModel<string> fieldViewModel;
                        if (field is SinglelineTextField)
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
                            fieldViewModel.AddValidator(
                                new RequiredValidator { ValidationMessage = field.ValidationMessage });
                        }

                        // add all other validators
                        foreach (var validator in field.Validators)
                        {
                            fieldViewModel.AddValidator(validator);
                        }

                        // todo: validators should handle format string -> i.e {0} in the validation message should be replaced with the field name

                        sectionViewModel.Fields.Add(fieldViewModel);
                    }

                    if (activeStep is MultiStep)
                    {
                        (step as MultiStepViewModel).NextStepUrl = activeStep.GetNextStepUrl();
                        (step as MultiStepViewModel).PreviousStepUrl = activeStep.GetPreviousStepUrl();
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