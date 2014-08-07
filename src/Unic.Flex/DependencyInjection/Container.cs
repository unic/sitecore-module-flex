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
        /// The kernel
        /// </summary>
        private static IKernel kernel;

        /// <summary>
        /// Creates the kernel.
        /// </summary>
        /// <param name="modules">The modules.</param>
        /// <returns>New instance of the kernel.</returns>
        public static IKernel CreateKernel(INinjectModule[] modules)
        {
            kernel = new StandardKernel(modules);
            return kernel;
        }

        /// <summary>
        /// Resolves an instance of given type from the container.
        /// </summary>
        /// <typeparam name="T">Type of the class to resolve</typeparam>
        /// <returns>Instance of the type</returns>
        public static T Resolve<T>()
        {
            return kernel.Get<T>();
        }
    }
}
