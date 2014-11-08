namespace Unic.Flex.Tests.Mapping
{
    using Moq;
    using Moq.Protected;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using Unic.Flex.Mapping;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Steps;
    using Unic.Flex.Model.ViewModel.Fields.InputFields;
    using Unic.Flex.Model.ViewModel.Forms;
    using Unic.Flex.Model.ViewModel.Steps;
    using Unic.Flex.Tests.Common;
    using Unic.Flex.Tests.Common.Moqs;

    /// <summary>
    /// Tests for model converting
    /// </summary>
    public class ModelConverterServiceTests
    {
        /// <summary>
        /// Test the converting from domain model to view model
        /// </summary>
        [TestFixture]
        public class TheConvertToViewModelMethod
        {
            /// <summary>
            /// Initializes the tests by configuring auto mapper.
            /// </summary>
            [TestFixtureSetUp]
            public void InitializeTests()
            {
                new AutoMapperMockConfig().Configure();
            }
            
            /// <summary>
            /// Converter should throw an exception if form could not be loaded
            /// </summary>
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void WillThrowExceptionWhenFormWasNotFound()
            {
                // prepare
                var form = this.GetForm(FormRepositoryMoqs.GetNullFormRepository());
                var converter = new ModelConverterService(SimpleMoqs.GetDictionaryRepository(), null, null, null);

                // act
                var viewModel = converter.ConvertToViewModel(form);

                // assert
                Assert.Fail("Failed, exception should be thrown");
            }

            /// <summary>
            /// Converter should return correct view model for a single step form
            /// </summary>
            [Test]
            public void WillReturnCorrectViewModelForSingleStepForm()
            {
                // prepare
                var form = this.GetForm(FormRepositoryMoqs.GetSingleStepFormRepository());
                var converter = new ModelConverterService(SimpleMoqs.GetDictionaryRepository(), null, SimpleMoqs.GetConfigurationManager(), null);

                // act
                var viewModel = converter.ConvertToViewModel(form);

                // assert
                Assert.NotNull(viewModel);
                Assert.NotNull(viewModel.Step);
                Assert.IsTrue(viewModel.Step.Sections.Count == form.GetSections().Count());
                Assert.IsTrue(viewModel.Step.Sections.First().Fields.Last().GetType() == typeof(HoneypotFieldViewModel));
                Assert.IsTrue(viewModel.Step.Sections.First().Fields.Count == form.GetSections().SelectMany(s => s.Fields).Count() + 1);
                Assert.IsTrue(viewModel.Step.Sections.First().Fields.First().Id == FormRepositoryMoqs.SingleLineTextGuid);
                Assert.IsTrue(viewModel.Step.Sections.First().Fields.First().ViewName == "Fields/InputFields/SinglelineText");
                Assert.IsTrue(viewModel.Step.Sections.First().Fields.First().CssClass.Trim() == "flex_singletextfield");
            }

            /// <summary>
            /// Converter should return correct view model for a multi step form
            /// </summary>
            [Test]
            public void WillReturnCorrectViewModelForMultiStepForm()
            {
                // prepare
                var form = this.GetForm(FormRepositoryMoqs.GetMultiStepFormRepository());
                var converter = new ModelConverterService(SimpleMoqs.GetDictionaryRepository(), null, SimpleMoqs.GetConfigurationManager(), null);

                // act
                var viewModel = converter.ConvertToViewModel(form);

                // assert
                Assert.NotNull(viewModel);
                Assert.NotNull(viewModel.Step);
                Assert.IsTrue(viewModel.Step.Sections.Count == form.GetActiveStep().Sections.Count());
                Assert.IsTrue(viewModel.Step.Sections.First().Fields.Last().GetType() == typeof(HoneypotFieldViewModel));
                Assert.IsTrue(viewModel.Step.Sections.First().Fields.Count == form.GetActiveStep().Sections.SelectMany(s => s.Fields).Count() + 1);
            }

            /// <summary>
            /// Converter should return correct summaryview model for a multi step form
            /// </summary>
            [Test]
            public void WillReturnCorrectViewModelForSummaryInMultiStepForm()
            {
                // prepare
                var form = this.GetForm(FormRepositoryMoqs.GetMultiStepFormRepository());
                form.Steps.Last().IsActive = true;
                var converter = new ModelConverterService(SimpleMoqs.GetDictionaryRepository(), null, SimpleMoqs.GetConfigurationManager(), null);

                // act
                var viewModel = converter.ConvertToViewModel(form);

                // assert
                Assert.NotNull(viewModel);
                Assert.NotNull(viewModel.Step);
                Assert.IsTrue(viewModel.Step.GetType() == typeof(SummaryViewModel));
                Assert.IsTrue(viewModel.Step.Sections.Count == form.Steps.OfType<MultiStep>().SelectMany(s => s.Sections).Count());
                Assert.IsTrue(viewModel.Step.Sections.SelectMany(s => s.Fields).Count() == form.Steps.OfType<MultiStep>().SelectMany(s => s.Sections).SelectMany(s => s.Fields).Count());
            }

            /// <summary>
            /// Gets the form.
            /// </summary>
            /// <param name="repository">The repository.</param>
            /// <returns>The form from the repository</returns>
            private Form GetForm(IFormRepository repository)
            {
                return repository.LoadForm(string.Empty);
            }
        }

        /// <summary>
        /// Test the get view model method
        /// </summary>
        [TestFixture]
        public class TheGetViewModelMethod
        {
            /// <summary>
            /// Test should thro an exception if view model was not found
            /// </summary>
            [Test]
            [ExpectedException(typeof(TypeLoadException))]
            public void WillThrowExceptionWhenViewModelWasNotFound()
            {
                // arrange
                var converter = new ModelConverterService(SimpleMoqs.GetDictionaryRepository(), null, null, null);
                var domainModel = new object();

                // act
                var viewModel = converter.GetViewModel<FormViewModel>(domainModel);

                // assert
                Assert.Fail("Failed, exception should be thrown");
            }

            /// <summary>
            /// Test should show an exception if the view model was from the wrong type.
            /// </summary>
            [Test]
            [ExpectedException(typeof(TypeLoadException))]
            public void WillThrowExceptionWhenViewModelWasWrongType()
            {
                // arrange
                var converter = new ModelConverterService(SimpleMoqs.GetDictionaryRepository(), null, null, null);
                var domainModel = new SingleStep();

                // act
                var viewModel = converter.GetViewModel<FormViewModel>(domainModel);

                // assert
                Assert.Fail("Failed, exception should be thrown");
            }

            /// <summary>
            /// Test should return correct view model instance.
            /// </summary>
            [Test]
            public void ShouldReturnCorrectViewModelInstance()
            {
                // arrange
                var converter = new ModelConverterService(SimpleMoqs.GetDictionaryRepository(), null, null, null);
                var domainModel = new Form();

                // act
                var viewModel = converter.GetViewModel<FormViewModel>(domainModel);

                // assert
                Assert.NotNull(viewModel);
                Assert.IsInstanceOf<FormViewModel>(viewModel);
            }

            /// <summary>
            /// Should return correct view model instance from change if loading twice.
            /// </summary>
            [Test]
            public void ShouldReturnCorrectViewModelInstanceFromCache()
            {
                // arrange
                var serviceMock = new Mock<ModelConverterService>(SimpleMoqs.GetDictionaryRepository(), null, null) { CallBase = true };
                var converter = serviceMock.Object;
                var domainModel = new Form();

                // act
                var firstViewModel = converter.GetViewModel<FormViewModel>(domainModel);
                var secondViewModel = converter.GetViewModel<FormViewModel>(domainModel);

                // assert
                Assert.NotNull(firstViewModel);
                Assert.NotNull(secondViewModel);
                Assert.IsInstanceOf<FormViewModel>(firstViewModel);
                Assert.IsInstanceOf<FormViewModel>(secondViewModel);
                Assert.AreNotSame(firstViewModel, secondViewModel);
                serviceMock.Protected().Verify("ResolveViewModelTypeFromAssembly", Times.Once(), ItExpr.IsAny<object>());
            }
        }
    }
}
