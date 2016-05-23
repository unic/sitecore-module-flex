namespace Unic.Flex.NinjectIoC
{
    using Sitecore.Pipelines;
    using Unic.Flex.Core.DependencyInjection;

    /// <summary>
    /// Ninject configuration activator
    /// </summary>
    public class Bootstrapper
    {
        /// <summary>
        /// Initialize the application
        /// </summary>
        public virtual void Process(PipelineArgs args)
        {
            DependencyResolver.SetContainer(new NinjectContainer());
        }
    }
}
