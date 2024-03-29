﻿namespace Unic.Flex.SimpleInjectorIoC
{
    using Sitecore.Pipelines;
    using Unic.Flex.Core.DependencyInjection;

    /// <summary>
    /// Simple injector configuration activator
    /// </summary>
    public class Bootstrapper
    {
        /// <summary>
        /// Initialize the application
        /// </summary>
        public virtual void Process(PipelineArgs args)
        {
            DependencyResolver.SetContainer(new SimpleInjectorContainer());
        }
    }
}
