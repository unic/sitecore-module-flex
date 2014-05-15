﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Unic.Flex.Website.Models.Flex
{
    using System.Web.Mvc;
    using Unic.Flex.DomainModel;
    using Unic.Flex.DomainModel.Fields;
    using Unic.Flex.DomainModel.Presentation;
    using Unic.Flex.Presentation;
    using Unic.Flex.Website.ModelBinding;

    public class FieldViewModel : IPresentationComponent, IViewModel
    {
        public FieldViewModel(ItemBase domainModel)
        {
            this.DomainModel = domainModel;
        }
        
        public string Key { get; set; }

        public string Label { get; set; }

        public object Value { get; set; }

        public string ViewName { get; set; }

        public ItemBase DomainModel { get; set; }
    }
}