namespace Unic.Flex.Website.Initialize
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Sitecore.Pipelines;
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
        /// Initialize the application..
        /// </summary>
        public virtual void Process(PipelineArgs args)
        {
            this.RegisterModelBinders();
            this.RegisterConfigurationConverters();
        }

        /// <summary>
        /// Registers the ASP.NET MVC model binders.
        /// </summary>
        protected virtual void RegisterModelBinders()
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
        protected virtual void RegisterConfigurationConverters()
        {
            ConverterFactory.RegisterConverter(new SpecificationConverter());
        }
    }
}