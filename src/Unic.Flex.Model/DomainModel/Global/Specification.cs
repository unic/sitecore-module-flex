namespace Unic.Flex.Model.DomainModel.Global
{
    using Glass.Mapper.Sc.Configuration.Attributes;

    /// <summary>
    /// A specification item
    /// </summary>
    [SitecoreType(TemplateId = "{47D4110F-7D21-4DC8-89F0-41302DB57637}")]
    public class Specification : ItemBase
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [SitecoreField("Value")]
        public virtual string Value { get; set; }
    }
}
