[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Unic.Flex.Website.AutoMapperConfig), "PreStart")]

namespace Unic.Flex.Website
{
    using AutoMapper;
    using Unic.Flex.Context;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Sections;
    using Unic.Flex.Model.DomainModel.Steps;
    using Unic.Flex.Model.ViewModel.Forms;
    using Unic.Flex.Model.ViewModel.Sections;
    using Unic.Flex.Model.ViewModel.Steps;

    public static class AutoMapperConfig
    {
        public static void PreStart()
        {
            Configure();
        }
        
        public static void Configure()
        {
            // Forms
            Mapper.CreateMap<Form, FormViewModel>().ForMember(m => m.Step, o => o.Ignore());

            // Steps
            Mapper.CreateMap<SingleStep, SingleStepViewModel>().ForMember(m => m.Sections, o => o.Ignore());
            Mapper.CreateMap<MultiStep, MultiStepViewModel>()
                .ForMember(m => m.Sections, o => o.Ignore())
                .ForMember(m => m.NextStepUrl, o => o.ResolveUsing(b => b.GetNextStepUrl()))
                .ForMember(m => m.PreviousStepUrl, o => o.ResolveUsing(b => b.GetPreviousStepUrl()));
            Mapper.CreateMap<Summary, SummaryViewModel>()
                .ForMember(m => m.Sections, o => o.Ignore())
                .ForMember(m => m.PreviousStepUrl, o => o.ResolveUsing(b => b.GetPreviousStepUrl()));

            // Sections
            Mapper.CreateMap<StandardSection, StandardSectionViewModel>().ForMember(m => m.Fields, o => o.Ignore());
        }
    }
}