[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Unic.Flex.Website.App_Start.AutoMapperConfig), "PostStart", Order = 1)]

namespace Unic.Flex.Website.App_Start
{
    using AutoMapper;
    using Unic.Flex.Core.Context;
    using Unic.Flex.Model.DomainModel.Sections;
    using Unic.Flex.Model.Entities;
    using Unic.Flex.Model.Steps;
    using Unic.Flex.Model.Types;
    using Unic.Flex.Model.ViewModel.Forms;
    using Unic.Flex.Model.ViewModel.Sections;
    using Unic.Flex.Model.ViewModel.Steps;
    using Form = Unic.Flex.Model.Forms.Form;

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
            // Forms
            Mapper.CreateMap<Form, FormViewModel>().ForMember(m => m.Step, o => o.Ignore());

            // Steps
            Mapper.CreateMap<SingleStep, SingleStepViewModel>().ForMember(m => m.Sections, o => o.Ignore());
            Mapper.CreateMap<MultiStep, MultiStepViewModel>().ForMember(m => m.Sections, o => o.Ignore());
            Mapper.CreateMap<Summary, SummaryViewModel>().ForMember(m => m.Sections, o => o.Ignore());

            // Sections
            Mapper.CreateMap<StandardSection, StandardSectionViewModel>().ForMember(m => m.Fields, o => o.Ignore());

            // Files
            Mapper.CreateMap<UploadedFile, File>();
        }
    }
}