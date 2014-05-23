using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Tests.ModelBinding
{
    using NUnit.Framework;
    using Unic.Flex.Mapping;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.ModelBinding;
    using Unic.Flex.Tests.Common.Moqs;

    public class ModelConverterServiceTests
    {
        [TestFixture]
        public class TheConvertToViewModelMethod
        {
            /// <summary>
            /// Converter should throw an exception if form could not be loaded
            /// </summary>
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void WillThrowExceptionWhenFormWasNotFound()
            {
                // prepare
                var form = this.GetForm(FormRepositoryMoqs.GetNullFormRepository());
                var converter = new ModelConverterService();

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
                var converter = new ModelConverterService();

                // act
                var viewModel = converter.ConvertToViewModel(form);

                // assert
                Assert.NotNull(viewModel);
                Assert.NotNull(viewModel.Step);

                // todo: add more test cases
            }

            private Form GetForm(IFormRepository repository)
            {
                return repository.LoadForm(string.Empty, null);
            }
        }
    }
}
