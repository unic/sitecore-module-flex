namespace Unic.Flex.Model.Fields
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.Presentation;

    public abstract class FieldBase : ItemBase, IPresentationComponent
    {
        public abstract string ViewName { get; }

        [SitecoreField("Label")]
        public virtual string Label { get; set; }

        public virtual object Value { get; set; }
    }
}
