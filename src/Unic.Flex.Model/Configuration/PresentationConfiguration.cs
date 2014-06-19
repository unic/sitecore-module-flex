namespace Unic.Flex.Model.Configuration
{
    using Sitecore.Data.Items;
    using Unic.Configuration;

    public class PresentationConfiguration : AbstractConfiguration
    {
        [Configuration(FieldName = "Theme")]
        public Item Theme { get; set; }
    }
}
