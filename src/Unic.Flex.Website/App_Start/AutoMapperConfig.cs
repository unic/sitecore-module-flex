[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Unic.Flex.Website.AutoMapperConfig), "PostStart")]

namespace Unic.Flex.Website
{
    using AutoMapper;
    using Unic.Flex.Context;
    using Unic.Flex.Model.DomainModel.Sections;
    using Unic.Flex.Model.DomainModel.Steps;
    using Unic.Flex.Model.Entities;
    using Unic.Flex.Model.Types;
    using Unic.Flex.Model.ViewModel.Forms;
    using Unic.Flex.Model.ViewModel.Sections;
    using Unic.Flex.Model.ViewModel.Steps;
    using Form = Unic.Flex.Model.DomainModel.Forms.Form;

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
            Mapper.CreateMap<MultiStep, MultiStepViewModel>()
                .ForMember(m => m.Sections, o => o.Ignore())
                .ForMember(m => m.NextStepUrl, o => o.ResolveUsing(this.GetNextStepUrl))
                .ForMember(m => m.PreviousStepUrl, o => o.ResolveUsing(this.GetPreviousStepUrl));
            Mapper.CreateMap<Summary, SummaryViewModel>()
                .ForMember(m => m.Sections, o => o.Ignore())
                .ForMember(m => m.PreviousStepUrl, o => o.ResolveUsing(this.GetPreviousStepUrl));

            // Sections
            Mapper.CreateMap<StandardSection, StandardSectionViewModel>().ForMember(m => m.Fields, o => o.Ignore());

            // Files
            Mapper.CreateMap<UploadedFile, File>();
        }

        /// <summary>
        /// Gets the next step URL.
        /// </summary>
        /// <param name="step">The step.</param>
        /// <returns>Url for the next step</returns>
        public virtual string GetNextStepUrl(StepBase step)
        {
            return step.GetNextStepUrl();
        }

        /// <summary>
        /// Gets the previous step URL.
        /// </summary>
        /// <param name="step">The step.</param>
        /// <returns>Url for the previous step</returns>
        public virtual string GetPreviousStepUrl(StepBase step)
        {
            return step.GetPreviousStepUrl();
        }
    }
}