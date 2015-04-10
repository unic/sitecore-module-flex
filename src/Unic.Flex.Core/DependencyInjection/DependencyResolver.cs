namespace Unic.Flex.Core.DependencyInjection
{
    using System;

    /// <summary>
    /// Dependency injection container.
    /// </summary>
    public static class DependencyResolver
    {
        /// <summary>
        /// The container
        /// </summary>
        private static IContainer container;

        /// <summary>
        /// Sets the kernel.
        /// </summary>
        /// <param name="newContainer">The new container.</param>
        public static void SetContainer(IContainer newContainer)
        {
            container = newContainer;
        }

        /// <summary>
        /// Verifies the container.
        /// </summary>
        /// <returns>Boolean result from verifying the container</returns>
        public static bool VerifyContainer()
        {
            return container.VerifyContainer();
        }

        /// <summary>
        /// Adds a new binding to the container.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        public static void Bind<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            container.Bind<TService, TImplementation>();
        }

        /// <summary>
        /// Resolves an instance of given type from the container.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>
        /// Instance of the type
        /// </returns>
        public static TService Resolve<TService>() where TService : class
        {
            return container.Resolve<TService>();
        }

        /// <summary>
        /// Resolves the specified type from the container.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>
        /// Instance of the type
        /// </returns>
        public static object Resolve(Type type)
        {
            return container.Resolve(type);
        }
    }
}
