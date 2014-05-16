namespace Unic.Flex.Model.Sections
{
    using System.Collections.Generic;
    using Unic.Flex.Model;
    using Unic.Flex.Model.Fields;
    using Unic.Flex.Model.Presentation;
    using Unic.Flex.Presentation;

    public class SectionViewModel : IPresentationComponent, IViewModel
    {
        public SectionViewModel(ItemBase domainModel)
        {
            this.Fields = new List<FieldViewModel>();
            this.DomainModel = domainModel;
        }
        
        public IList<FieldViewModel> Fields { get; private set; }

        public bool DisableFieldset { get; set; }

        public string Title { get; set; }
        
        public string ViewName { get; set; }

        public ItemBase DomainModel { get; set; }
    }
}