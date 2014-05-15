using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Unic.Flex.Website.Models.Flex
{
    using Unic.Flex.DomainModel;
    using Unic.Flex.DomainModel.Presentation;
    using Unic.Flex.DomainModel.Sections;
    using Unic.Flex.Presentation;

    public class SectionViewModel : IPresentationComponent, IViewModel
    {
        public SectionViewModel(ItemBase model)
        {
            this.Fields = new List<FieldViewModel>();
            this.DomainModel = model;
        }
        
        public IList<FieldViewModel> Fields { get; private set; }

        public bool DisableFieldset { get; set; }

        public string Title { get; set; }
        
        public string ViewName { get; set; }

        public ItemBase DomainModel { get; set; }
    }
}