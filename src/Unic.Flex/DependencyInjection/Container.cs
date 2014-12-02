namespace Unic.Flex.DependencyInjection
{
    using Ninject;
    using Ninject.Modules;
    using System;
    using Ninject.Parameters;

    /// <summary>
    /// Dependency injection container.
    /// </summary>
    public static class Container
    {
        /// <summary>
        /// The flex parameter
        /// </summary>
        private static readonly IParameter IsFlexParameter = new Parameter("is_flex", true, true);

        /// <summary>
        /// The parameters
        /// </summary>
        private static readonly IParameter[] Parameters = { FlexParameter };
        
        /// <summary>
        /// The kernel
        /// </summary>
        private static IKernel kernel;

        /// <summary>
        /// Gets the flex parameter.
        /// </summary>
        /// <value>
        /// The flex parameter.
        /// </value>
        public static IParameter FlexParameter
        {
            get
            {
                return IsFlexParameter;
            }
        }

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
        /// <param name="name">The name.</param>
        /// <returns>
        /// Instance of the type
        /// </returns>
        public static T Resolve<T>(string name = null)
        {
            return kernel.Get<T>(name, Parameters);
        }

        /// <summary>
        /// Resolves the specified type from the container.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Instance of the type
        /// </returns>
        public static object Resolve(Type type, string name = null)
        {
            return kernel.Get(type, name, Parameters);
        }
    }
}
