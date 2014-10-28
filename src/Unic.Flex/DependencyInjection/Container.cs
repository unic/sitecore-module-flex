namespace Unic.Flex.DependencyInjection
{
    using Ninject;
    using Ninject.Modules;
    using System;

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
        /// Sets the kernel.
        /// </summary>
        /// <param name="newKernel">The new kernel.</param>
        public static void SetKernel(IKernel newKernel)
        {
            kernel = newKernel;
        }

        /// <summary>
        /// Resolves an instance of given type from the container.
        /// </summary>
        /// <typeparam name="T">Type of the class to resolve</typeparam>
        /// <param name="name">The name of the binding.</param>
        /// <returns>
        /// Instance of the type
        /// </returns>
        public static T Resolve<T>(string name = null)
        {
            return kernel.Get<T>(name);
        }

        /// <summary>
        /// Resolves the specified type from the container.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <param name="name">The name of the binding.</param>
        /// <returns>
        /// Instance of the type
        /// </returns>
        public static object Resolve(Type type, string name = null)
        {
            return kernel.Get(type, name);
        }
    }
}
