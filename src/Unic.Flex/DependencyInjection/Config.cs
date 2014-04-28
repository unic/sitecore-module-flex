namespace Unic.Flex.DependencyInjection
{
    using Ninject.Modules;
    using Unic.Flex.Context;
    using Unic.Flex.Mapping;

    /// <summary>
    /// Ninject configuration module.
    /// </summary>
    public class Config : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IContextService>().To<ContextService>();
            Bind<IFormRepository>().To<FormRepository>();
        }
    }
}
