using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Unic.Flex.Website.Models.Flex
{
    using Unic.Flex.DomainModel;
    using Unic.Flex.DomainModel.Presentation;
    using Unic.Flex.DomainModel.Sections;
    using Unic.Flex.DomainModel.Steps;
    using Unic.Flex.Presentation;

    public class StepViewModel : IPresentationComponent, IViewModel
    {
        public StepViewModel(ItemBase model)
        {
            this.Sections = new List<SectionViewModel>();
            this.DomainModel = model;
        }
        
        public IList<SectionViewModel> Sections { get; private set; }

        public string ViewName { get; set; }

        public ItemBase DomainModel { get; set; }
    }
}