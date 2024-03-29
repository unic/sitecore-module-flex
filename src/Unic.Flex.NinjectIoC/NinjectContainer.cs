﻿namespace Unic.Flex.NinjectIoC
{
    using System;
    using Ninject;
    using Unic.Flex.Core.DependencyInjection;

    /// <summary>
    /// Ninject IoC container
    /// </summary>
    public class NinjectContainer : IContainer
    {
        /// <summary>
        /// The kernel
        /// </summary>
        private readonly IKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectContainer"/> class.
        /// </summary>
        public NinjectContainer() : this (CreateStandardKernel())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectContainer"/> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public NinjectContainer(IKernel kernel)
        {
            // set the kernel
            this.kernel = kernel;

            // configure dependencies
            this.kernel.Load(new Config());
        }

        /// <summary>
        /// Verifies the container.
        /// </summary>
        /// <returns>
        /// Boolean result from verifying the container
        /// </returns>
        /// <exception cref="System.NotImplementedException">Currently not available with Ninject</exception>
        public bool VerifyContainer()
        {
            throw new NotImplementedException("Currently not available with Ninject");
        }

        /// <summary>
        /// Adds a new binding to the container.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        public void Bind<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            var instance = this.kernel.TryGet<TService>();
            if (instance == null)
            {
                this.kernel.Bind<TService>().To<TImplementation>();
            }
            else
            {
                this.kernel.Rebind<TService>().To<TImplementation>();
            }
        }

        /// <summary>
        /// Resolves an instance of given type from the container.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>
        /// Instance of the type
        /// </returns>
        public TService Resolve<TService>() where TService : class
        {
            return this.kernel.Get<TService>();
        }

        /// <summary>
        /// Resolves the specified type from the container.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>
        /// Instance of the type
        /// </returns>
        public object Resolve(Type type)
        {
            return this.kernel.Get(type);
        }

        public object BeginScope()
        {
            return null;
        }

        public void EndScope(object scope)
        {
        }

        /// <summary>
        /// Creates a standard kernel.
        /// </summary>
        /// <returns>Dependency inection container</returns>
        private static IKernel CreateStandardKernel()
        {
            return new StandardKernel();
        }
    }
}
