﻿namespace Unic.Flex.NinjectIoC
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
        /// Adds a new binding to the container.
        /// </summary>
        /// <typeparam name="TFrom">The type of bind.</typeparam>
        /// <typeparam name="TTo">The type of resolve.</typeparam>
        public void Bind<TFrom, TTo>() where TTo : TFrom
        {
            var instance = this.kernel.TryGet<TFrom>();
            if (instance == null)
            {
                this.kernel.Bind<TFrom>().To<TTo>();
            }
            else
            {
                this.kernel.Rebind<TFrom>().To<TTo>();
            }
        }

        /// <summary>
        /// Resolves an instance of given type from the container.
        /// </summary>
        /// <typeparam name="T">Type of the class to resolve</typeparam>
        /// <returns>
        /// Instance of the type
        /// </returns>
        public T Resolve<T>()
        {
            return this.kernel.Get<T>();
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
    }
}
