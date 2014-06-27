namespace Unic.Flex.Tests.Common.Moqs
{
    using System;
    using System.Collections.Generic;
    using Glass.Mapper.Sc;
    using Moq;
    using Unic.Flex.Mapping;
    using Unic.Flex.Model.DomainModel.Fields;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Sections;
    using Unic.Flex.Model.DomainModel.Steps;

    public static class FormRepositoryMoqs
    {
        public static IFormRepository GetNullFormRepository()
        {
            return GetMoq(GetNullForm);
        }

        public static IFormRepository GetSingleStepFormRepository()
        {
            return GetMoq(GetSingleStepForm);
        }
        
        private static IFormRepository GetMoq(Func<Form> formLoader)
        {
            var mock = new Mock<IFormRepository>();
            mock.Setup(method => method.LoadForm(It.IsAny<string>(), It.IsAny<ISitecoreContext>())).Returns(formLoader);
            return mock.Object;
        }

        private static Form GetNullForm()
        {
            return null;
        }

        private static Form GetSingleStepForm()
        {
            // generate the sections
            var firstSection = new StandardSection();
            firstSection.Title = "This is the first section";
            firstSection.Fields = new List<IField>();
            
            var secondSection = new StandardSection();
            secondSection.Title = "This is the second section";
            secondSection.Fields = new List<IField>();

            // generate the step
            var step = new SingleStep();
            step.StepNumber = 1;
            step.Sections = new List<SectionBase> { firstSection, secondSection };

            // generate the form
            var form = new Form();
            form.Title = "Single Step Form";
            form.Introduction = "This is the introduction for a single step form";
            form.ItemId = Guid.NewGuid();
            form.Language = "en";
            form.Steps = new List<StepBase> { step };
            return form;
        }
    }
}
