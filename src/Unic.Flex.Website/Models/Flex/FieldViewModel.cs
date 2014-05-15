using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Unic.Flex.Website.Models.Flex
{
    using Unic.Flex.DomainModel;
    using Unic.Flex.DomainModel.Fields;
    using Unic.Flex.DomainModel.Presentation;
    using Unic.Flex.Presentation;

    public class FieldViewModel : IPresentationComponent, IViewModel
    {
        public FieldViewModel(ItemBase model)
        {
            this.DomainModel = model;
        }
        
        public string Key { get; set; }

        public string Label { get; set; }

        public object Value { get; set; }

        public string ViewName { get; set; }

        public ItemBase DomainModel { get; set; }
    }
}