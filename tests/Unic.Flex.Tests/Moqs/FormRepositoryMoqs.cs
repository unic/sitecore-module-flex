﻿namespace Unic.Flex.Tests.Moqs
{
    using System;
    using System.Collections.Generic;
    using Moq;
    using Unic.Flex.Core.Mapping;
    using Unic.Flex.Implementation.Fields.InputFields;
    using Unic.Flex.Model.Fields;
    using Unic.Flex.Model.Forms;
    using Unic.Flex.Model.Sections;
    using Unic.Flex.Model.Steps;
    using Unic.Flex.Model.Validators;

    /// <summary>
    /// Class to generate form reqpository Moqs
    /// </summary>
    public static class FormRepositoryMoqs
    {
        /// <summary>
        /// The single line text unique identifier
        /// </summary>
        public const string SingleLineTextGuid = "eb0668c1-ba61-47f8-8a6b-084f19b8575a";

        /// <summary>
        /// The multi line text unique identifier
        /// </summary>
        public const string MultiLineTextGuid = "6998c09b-44ea-4aff-9c68-29dad6ada959";
        
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
        /// Gets the multi step form repository.
        /// </summary>
        /// <returns>A repository with a multi step form</returns>
        public static IFormRepository GetMultiStepFormRepository()
        {
            return GetMoq(GetMultiStepForm);
        }

        /// <summary>
        /// Gets the moq.
        /// </summary>
        /// <param name="formLoader">The form loader function.</param>
        /// <returns>The generated form repository moq</returns>
        private static IFormRepository GetMoq(Func<Form> formLoader)
        {
            var mock = new Mock<IFormRepository>();
            mock.Setup(method => method.LoadForm(It.IsAny<string>(), It.IsAny<bool>())).Returns(formLoader);
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
            // generate fields
            var fields = new List<IField>();
            fields.Add(new SinglelineTextField { Validators = new List<IValidator>(), ItemId = new Guid(SingleLineTextGuid) });
            fields.Add(new MultilineTextField { Validators = new List<IValidator>(), ItemId = new Guid(MultiLineTextGuid) });
            
            // generate the sections
            var firstSection = new StandardSection();
            firstSection.Title = "This is the first section";
            firstSection.LazyFields = fields;
            
            var secondSection = new StandardSection();
            secondSection.Title = "This is the second section";
            secondSection.LazyFields = fields;

            // generate the step
            var step = new SingleStep();
            step.StepNumber = 1;
            step.Url = "step";
            step.LazySections = new List<StandardSection> { firstSection, secondSection };

            // generate the form
            var form = new Form();
            form.Title = "Single Step Form";
            form.Introduction = "This is the introduction for a single step form";
            form.ItemId = Guid.NewGuid();
            form.Language = "en";
            form.Steps = new List<StepBase> { step };
            return form;
        }

        /// <summary>
        /// Gets the multi step form.
        /// </summary>
        /// <returns>A multi step form</returns>
        private static Form GetMultiStepForm()
        {
            // generate fields
            var fields = new List<IField>();
            fields.Add(new SinglelineTextField { Validators = new List<IValidator>(), Key = "singlelinetext", ShowInSummary = true });
            fields.Add(new MultilineTextField { Validators = new List<IValidator>(), Key = "multilinetext", ShowInSummary = true });

            // generate section
            var section = new StandardSection();
            section.Title = "This is the section";
            section.LazyFields = fields;

            // generate the steps
            var steps = new List<StepBase>();
            steps.Add(new MultiStep { StepNumber = 1, Url = "first", LazySections = new List<StandardSection> { section } });
            steps.Add(new MultiStep { StepNumber = 2, Url = "second", LazySections = new List<StandardSection> { section } });
            steps.Add(new Summary { StepNumber = 3, Url = "summary", LazySections = new List<StandardSection>() });

            // generate the form
            var form = new Form();
            form.Title = "Multi Step Form";
            form.Introduction = "This is the introduction for a multi step form";
            form.ItemId = Guid.NewGuid();
            form.Language = "en";
            form.Steps = steps;
            return form;
        }
    }
}
