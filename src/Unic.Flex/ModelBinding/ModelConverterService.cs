namespace Unic.Flex.ModelBinding
{
    using Unic.Flex.Model.Fields;
    using Unic.Flex.Model.Forms;
    using Unic.Flex.Model.Sections;
    using Unic.Flex.Model.Steps;

    public class ModelConverterService : IModelConverterService
    {
        public FormViewModel ConvertToViewModel(Form form)
        {
            var activeStep = form.GetActiveStep();
            var step = new StepViewModel(activeStep) { ViewName = activeStep.ViewName };
            if (activeStep is StandardStep)
            {
                foreach (var section in ((StandardStep)activeStep).Sections)
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