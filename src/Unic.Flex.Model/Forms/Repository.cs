namespace Unic.Flex.Model.Forms
{
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.Configuration.Attributes;

    /// <summary>
    /// A forms repository
    /// </summary>
    [SitecoreType(TemplateId = "{7212DD21-1769-43AA-AF6A-92756C97A269}")]
    public class Repository
    {
        /// <summary>
        /// Gets or sets the full path.
        /// </summary>
        /// <value>
        /// The full path.
        /// </value>
        [SitecoreInfo(SitecoreInfoType.FullPath)]
        public virtual string FullPath { get; set; }
    }
}
