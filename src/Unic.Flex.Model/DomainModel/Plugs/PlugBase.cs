namespace Unic.Flex.Model.DomainModel.Plugs
{
    using Unic.Flex.Model.GlassExtensions.Attributes;

    public abstract class PlugBase : ItemBase
    {
        [SitecoreDictionaryFallbackField("Label", "Default plug error")]
        public virtual string ErrorMessage { get; set; }
    }
}
