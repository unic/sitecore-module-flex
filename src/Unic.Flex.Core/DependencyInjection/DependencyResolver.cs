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
        /// Resolves an instance of given type from the container.
        /// </summary>
        /// <typeparam name="T">Type of the class to resolve</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Instance of the type
        /// </returns>
        public static T Resolve<T>(string name = null)
        {
            return container.Resolve<T>(name);
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
            return container.Resolve(type, name);
        }
    }
}
