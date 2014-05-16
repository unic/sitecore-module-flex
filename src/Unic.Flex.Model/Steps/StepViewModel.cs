namespace Unic.Flex.Model.Steps
{
    using System.Collections.Generic;
    using Unic.Flex.Model;
    using Unic.Flex.Model.Presentation;
    using Unic.Flex.Model.Sections;
    using Unic.Flex.Presentation;

    public class StepViewModel : IPresentationComponent, IViewModel
    {
        public StepViewModel(ItemBase domainModel)
        {
            this.Sections = new List<SectionViewModel>();
            this.DomainModel = domainModel;
        }
        
        public IList<SectionViewModel> Sections { get; private set; }

        public string ViewName { get; set; }

        public ItemBase DomainModel { get; set; }
    }
}