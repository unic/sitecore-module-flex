namespace Unic.Flex.Model.Fields
{
    using System.Collections.Generic;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.Presentation;
    using Unic.Flex.Model.Validators;

    public abstract class FieldBase : ItemBase, IPresentationComponent
    {
        public abstract string ViewName { get; }

        [SitecoreField("Label")]
        public virtual string Label { get; set; }

        [SitecoreField("Is Required")]
        public virtual bool IsRequired { get; set; }

        [SitecoreField("Validation Message")]
        public virtual string ValidationMessage { get; set; }

        [SitecoreQuery("./Validators/*", InferType = true, IsRelative = true, IsLazy = true)]
        public virtual IEnumerable<IValidator> Validators { get; set; } 

        public virtual object Value { get; set; }
    }
}
