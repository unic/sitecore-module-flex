using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.DomainModel.Steps
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.DomainModel.Sections;

    public abstract class StandardStep : ItemBase, IStandardStep
    {
        [SitecoreChildren(IsLazy = true, InferType = true)]
        public virtual IEnumerable<ISectionBase> Sections { get; set; }
    }
}
