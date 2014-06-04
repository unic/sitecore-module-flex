﻿namespace Unic.Flex.DependencyInjection
{
    using System.Web.Mvc;
    using Ninject.Modules;
    using Unic.Flex.Context;
    using Unic.Flex.Globalization;
    using Unic.Flex.Mapping;
    using Unic.Flex.ModelBinding;
    using Unic.Flex.Plugs;
    using Unic.Flex.Presentation;

    /// <summary>
    /// Ninject configuration module.
    /// </summary>
    public class Config : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            // business logic
            Bind<IContextService>().To<ContextService>();
            Bind<IPresentationService>().To<PresentationService>();
            Bind<IPlugsService>().To<PlugsService>();
            
            // data access
            Bind<IDictionaryRepository>().To<DictionaryRepository>();
            Bind<IFormRepository>().To<FormRepository>();
            Bind<IUserDataRepository>().To<UserDataRepository>();

            // model binding and converting
            Bind<IModelBinder>().To<FormModelBinder>();
            Bind<IModelConverterService>().To<ModelConverterService>();
        }
    }
}
