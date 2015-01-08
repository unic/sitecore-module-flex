namespace Unic.Flex.Tests
{
    using Unic.Flex.Model.DomainModel.Steps;
    using Unic.Flex.Website.App_Start;

    /// <summary>
    /// Auto Mapper config mock for using in unit tests.
    /// </summary>
    public class AutoMapperMockConfig : AutoMapperConfig
    {
        /// <summary>
        /// Gets the next step URL.
        /// </summary>
        /// <param name="step">The step.</param>
        /// <returns>
        /// Url for the next step
        /// </returns>
        public override string GetNextStepUrl(StepBase step)
        {
            return string.Empty;
        }

        /// <summary>
        /// Gets the previous step URL.
        /// </summary>
        /// <param name="step">The step.</param>
        /// <returns>
        /// Url for the previous step
        /// </returns>
        public override string GetPreviousStepUrl(StepBase step)
        {
            return string.Empty;
        }
    }
}
