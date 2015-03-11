[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Unic.Flex.Website.App_Start.AutoMapperConfig), "PostStart", Order = 1)]

namespace Unic.Flex.Website.App_Start
{
    using AutoMapper;
    using Unic.Flex.Model.Entities;
    using Unic.Flex.Model.Types;

    /// <summary>
    /// Configuration for AutoMapper module.
    /// </summary>
    public class AutoMapperConfig
    {
        /// <summary>
        /// Executes after application has been started.
        /// </summary>
        public static void PostStart()
        {
            new AutoMapperConfig().Configure();
        }

        /// <summary>
        /// Configures the AutoMapper.
        /// </summary>
        public virtual void Configure()
        {
            // Files
            Mapper.CreateMap<UploadedFile, File>();
        }
    }
}