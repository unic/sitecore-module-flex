namespace Unic.Flex.Model.GlassExtensions.Configurations
{
    using Glass.Mapper.Sc.Configuration;

    /// <summary>
    /// Configuration for fields which can be reusable.
    /// </summary>
    public class SitecoreReusableFieldConfiguration : SitecoreFieldConfiguration
    {
        /// <summary>
        /// Makes a copy of the SitecoreFieldConfiguration
        /// </summary>
        /// <returns>
        /// The field configuration..
        /// </returns>
        public override SitecoreFieldConfiguration Copy()
        {
            return new SitecoreReusableFieldConfiguration
            {
                CodeFirst = this.CodeFirst,
                FieldId = this.FieldId,
                FieldName = this.FieldName,
                FieldSource = this.FieldSource,
                FieldTitle = this.FieldTitle,
                FieldType = this.FieldType,
                IsShared = this.IsShared,
                IsUnversioned = this.IsUnversioned,
                PropertyInfo = this.PropertyInfo,
                ReadOnly = this.ReadOnly,
                SectionName = this.SectionName,
                Setting = this.Setting
            };
        }
    }
}
