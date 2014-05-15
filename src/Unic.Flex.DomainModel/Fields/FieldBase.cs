using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.DomainModel.Fields
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.DomainModel.Presentation;

    public abstract class FieldBase : ItemBase, IPresentationComponent
    {
        public abstract string ViewName { get; }

        [SitecoreField("Label")]
        public virtual string Label { get; set; }
    }
}
