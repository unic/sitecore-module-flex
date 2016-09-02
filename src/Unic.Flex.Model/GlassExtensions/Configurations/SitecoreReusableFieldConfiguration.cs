namespace Unic.Flex.Model.GlassExtensions.Configurations
{
    using Glass.Mapper.Configuration;
    using Glass.Mapper.Sc.Configuration;

    /// <summary>
    /// Configuration for fields which can be reusable.
    /// </summary>
    public class SitecoreReusableFieldConfiguration : SitecoreFieldConfiguration
    {
        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new SitecoreReusableFieldConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as SitecoreReusableFieldConfiguration;
            base.Copy(copy);
        }
    }
}
