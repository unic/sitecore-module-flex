namespace Unic.Flex.Tests.Mapping
{
    using System;
    using Moq;
    using Moq.Protected;
    using NUnit.Framework;
    using Unic.Flex.Mapping;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Steps;
    using Unic.Flex.Model.ViewModel.Forms;
    using Unic.Flex.Tests.Common.Moqs;
    using Unic.Flex.Website;

    public class ModelConverterServiceTests
    {
        [TestFixture]
        public class TheConvertToViewModelMethod
        {
            /// <summary>
            /// Initializes the tests by configuring auto mapper.
            /// </summary>
            [TestFixtureSetUp]
            public void InitializeTests()
            {
                AutoMapperConfig.Configure();
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
                var converter = new ModelConverterService(SimpleMoqs.GetDictionaryRepository(), null);

                // act
                var viewModel = converter.ConvertToViewModel(form);

                // assert
                Assert.Fail("Failed, exception should be thrown");
            }

            [Test]
            public void WillReturnCorrectViewModelForSingleStepForm()
            {
                // prepare
                var form = this.GetForm(FormRepositoryMoqs.GetSingleStepFormRepository());
                var converter = new ModelConverterService(SimpleMoqs.GetDictionaryRepository(), null);

                // act
                var viewModel = converter.ConvertToViewModel(form);

                // assert
                Assert.NotNull(viewModel);
                Assert.NotNull(viewModel.Step);

                // todo: add more test cases
            }

            private Form GetForm(IFormRepository repository)
            {
                return repository.LoadForm(string.Empty);
            }
        }

        [TestFixture]
        public class TheGetViewModelMethod
        {
            [Test]
            [ExpectedException(typeof(TypeLoadException))]
            public void WillThrowExceptionWhenViewModelWasNotFound()
            {
                // arrange
                var converter = new ModelConverterService(SimpleMoqs.GetDictionaryRepository(), null);
                var domainModel = new object();

                // act
                var viewModel = converter.GetViewModel<FormViewModel>(domainModel);

                // assert
                Assert.Fail("Failed, exception should be thrown");
            }

            [Test]
            [ExpectedException(typeof(TypeLoadException))]
            public void WillThrowExceptionWhenViewModelWasWrongType()
            {
                // arrange
                var converter = new ModelConverterService(SimpleMoqs.GetDictionaryRepository(), null);
                var domainModel = new SingleStep();

                // act
                var viewModel = converter.GetViewModel<FormViewModel>(domainModel);

                // assert
                Assert.Fail("Failed, exception should be thrown");
            }

            [Test]
            public void ShouldReturnCorrectViewModelInstance()
            {
                // arrange
                var converter = new ModelConverterService(SimpleMoqs.GetDictionaryRepository(), null);
                var domainModel = new Form();

                // act
                var viewModel = converter.GetViewModel<FormViewModel>(domainModel);

                // assert
                Assert.NotNull(viewModel);
                Assert.IsInstanceOf<FormViewModel>(viewModel);
            }

            [Test]
            public void ShouldReturnCorrectViewModelInstanceFromCache()
            {
                // arrange
                var serviceMock = new Mock<ModelConverterService>(SimpleMoqs.GetDictionaryRepository(), null) { CallBase = true };
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
