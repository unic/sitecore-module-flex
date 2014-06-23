namespace Unic.Flex.Model.ViewModel.Steps
{
    using System.Collections.Generic;
    using Unic.Flex.Model.Presentation;
    using Unic.Flex.Model.ViewModel.Sections;

    public interface IStepViewModel : IPresentationComponent
    {
        IList<ISectionViewModel> Sections { get; }

        string Title { get; set; }
    }
}
