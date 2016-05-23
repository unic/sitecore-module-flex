﻿[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Unic.Flex.Website.App_Start.Bootstrapper), "PostStart", Order = 1)]

namespace Unic.Flex.Website.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Unic.Configuration.Core.Converter;
    using Unic.Flex.Core.ModelBinding;
    using Unic.Flex.Model.Configuration.Converters;
    using Unic.Flex.Model.Fields;
    using Unic.Flex.Model.Forms;
    using Unic.Flex.Model.Sections;
    using Unic.Flex.Model.Types;
    using DependencyResolver = Unic.Flex.Core.DependencyInjection.DependencyResolver;

    /// <summary>
    /// Bootstrapper for the Flex form module.
    /// </summary>
    public class Bootstrapper
    {
        /// <summary>
        /// The method called after starting the application.
        /// </summary>
        public static void PostStart()
        {
            RegisterModelBinders();
            RegisterConfigurationConverters();
        }

        /// <summary>
        /// Registers the ASP.NET MVC model binders.
        /// </summary>
        private static void RegisterModelBinders()
        {
            ModelBinders.Binders.Add(typeof(IForm), DependencyResolver.Resolve<FormModelBinder>());
            ModelBinders.Binders.Add(typeof(IList<ISection>), DependencyResolver.Resolve<ListModelBinder>());
            ModelBinders.Binders.Add(typeof(IList<IField>), DependencyResolver.Resolve<ListModelBinder>());
            ModelBinders.Binders.Add(typeof(IList<SelectListItem>), DependencyResolver.Resolve<ListModelBinder>());
            ModelBinders.Binders.Add(typeof(IField), DependencyResolver.Resolve<FieldModelBinder>());
            ModelBinders.Binders.Add(typeof(UploadedFile), DependencyResolver.Resolve<UploadedFileModelBinder>());
            ModelBinders.Binders.Add(typeof(DateTime?), DependencyResolver.Resolve<DateTimeModelBinder>());
            ModelBinders.Binders.Add(typeof(decimal?), DependencyResolver.Resolve<DecimalModelBinder>());
        }

        /// <summary>
        /// Registers the configuration converters for the config module.
        /// </summary>
        private static void RegisterConfigurationConverters()
        {
            ConverterFactory.RegisterConverter(new SpecificationConverter());
        }
    }
}