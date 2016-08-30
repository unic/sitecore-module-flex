namespace Unic.Flex.SimpleInjectorIoC
{
    using Sitecore.Pipelines;
    using Unic.Flex.Core.DependencyInjection;

    /// <summary>
    /// Simple injector configuration verification
    /// </summary>
    public class Verify
    {
        /// <summary>
        /// Verifies the current container configuration.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public virtual void Process(PipelineArgs args)
        {
            DependencyResolver.VerifyContainer();
        }
    }
}