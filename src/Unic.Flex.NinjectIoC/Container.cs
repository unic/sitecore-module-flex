namespace Unic.Flex.NinjectIoC
{
    using System;
    using Ninject;
    using Unic.Flex.Core.DependencyInjection;

    /// <summary>
    /// Ninject IoC container
    /// </summary>
    public class Container : IContainer
    {
        /// <summary>
        /// The kernel
        /// </summary>
        private readonly IKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="Container"/> class.
        /// </summary>
        public Container()
        {
            this.kernel = new StandardKernel(new Config());
        }

        /// <summary>
        /// Resolves an instance of given type from the container.
        /// </summary>
        /// <typeparam name="T">Type of the class to resolve</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Instance of the type
        /// </returns>
        public T Resolve<T>(string name = null)
        {
            return this.kernel.Get<T>(name);
        }

        /// <summary>
        /// Resolves the specified type from the container.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Instance of the type
        /// </returns>
        public object Resolve(Type type, string name = null)
        {
            return this.kernel.Get(type, name);
        }
    }
}
