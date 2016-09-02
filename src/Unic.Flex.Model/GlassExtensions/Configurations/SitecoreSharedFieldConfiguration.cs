namespace Unic.Flex.Model.GlassExtensions.Configurations
{
    using Glass.Mapper.Configuration;
    using Glass.Mapper.Sc.Configuration;

    /// <summary>
    /// Configuration for a shared field reference.
    /// </summary>
    public class SitecoreSharedFieldConfiguration : SitecoreFieldConfiguration
    {
        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new SitecoreSharedFieldConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as SitecoreSharedFieldConfiguration;
            base.Copy(copy);
        }
    }
}
