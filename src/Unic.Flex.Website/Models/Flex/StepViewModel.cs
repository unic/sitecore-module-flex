using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Unic.Flex.Website.Models.Flex
{
    using System.Web.Mvc;
    using Unic.Flex.Model;
    using Unic.Flex.Model.Presentation;
    using Unic.Flex.Presentation;
    using Unic.Flex.Website.ModelBinding;

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