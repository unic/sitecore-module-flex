namespace Unic.Flex.DependencyInjection
{
    using Ninject;
    using Ninject.Modules;

    /// <summary>
    /// Dependency injection container.
    /// </summary>
    public static class Container
    {
        /// <summary>
        /// Gets the kernel.
        /// </summary>
        /// <value>
        /// The kernel.
        /// </value>
        public static IKernel Kernel { get; private set; }

        /// <summary>
        /// Creates the kernel.
        /// </summary>
        /// <param name="modules">The modules.</param>
        /// <returns>New instance of the kernel.</returns>
        public static IKernel CreateKernel(INinjectModule[] modules)
        {
            Kernel = new StandardKernel(modules);
            return Kernel;
        }
    }
}
