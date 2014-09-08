namespace Unic.Flex.Tests.Common.Moqs
{
    using Moq;
    using System;
    using System.Collections.Generic;
    using Unic.Flex.Mapping;
    using Unic.Flex.Model.DomainModel.Fields;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Sections;
    using Unic.Flex.Model.DomainModel.Steps;

    /// <summary>
    /// Class to generate form reqpository Moqs
    /// </summary>
    public static class FormRepositoryMoqs
    {
        /// <summary>
        /// Gets the null form repository.
        /// </summary>
        /// <returns>A repository which return null</returns>
        public static IFormRepository GetNullFormRepository()
        {
            return GetMoq(GetNullForm);
        }

        /// <summary>
        /// Gets the single step form repository.
        /// </summary>
        /// <returns>A repository with a single step form</returns>
        public static IFormRepository GetSingleStepFormRepository()
        {
            return GetMoq(GetSingleStepForm);
        }

        /// <summary>
        /// Gets the moq.
        /// </summary>
        /// <param name="formLoader">The form loader function.</param>
        /// <returns>The generated form repository moq</returns>
        private static IFormRepository GetMoq(Func<Form> formLoader)
        {
            var mock = new Mock<IFormRepository>();
            mock.Setup(method => method.LoadForm(It.IsAny<string>())).Returns(formLoader);
            return mock.Object;
        }

        /// <summary>
        /// Gets the null form.
        /// </summary>
        /// <returns>Simply null</returns>
        private static Form GetNullForm()
        {
            return null;
        }

        /// <summary>
        /// Gets the single step form.
        /// </summary>
        /// <returns>A single step form</returns>
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
            step.Sections = new List<StandardSection> { firstSection, secondSection };

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
