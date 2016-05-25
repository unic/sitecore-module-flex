namespace Unic.Flex.SimpleInjectorIoC
{
    using Sitecore.Pipelines;
    using Unic.Flex.Core.DependencyInjection;

    /// <summary>
    /// Simple injector configuration activator
    /// </summary>
    public class Bootstrapper
    {
        /// <summary>
        /// Initialize the application
        /// </summary>
        public virtual void Process(PipelineArgs args)
        {
            this.Configure();
            this.Verify();
        }

        /// <summary>
        /// Configures the container.
        /// </summary>
        protected virtual void Configure()
        {
            DependencyResolver.SetContainer(new SimpleInjectorContainer());
        }

        /// <summary>
        /// Verifies the configuration.
        /// </summary>
        protected virtual void Verify()
        {
            DependencyResolver.VerifyContainer();
        }
    }
}
