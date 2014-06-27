namespace Unic.Flex.Model.GlassExtensions.Configurations
{
    using Glass.Mapper.Sc.Configuration;

    /// <summary>
    /// Configuration for a shared field reference.
    /// </summary>
    public class SitecoreSharedFieldConfiguration : SitecoreFieldConfiguration
    {
        /// <summary>
        /// Makes a copy of the SitecoreFieldConfiguration. This is used for enumeration types.
        /// </summary>
        /// <returns>
        /// SitecoreSharedFieldConfiguration for shared references.
        /// </returns>
        public override SitecoreFieldConfiguration Copy()
        {
            return new SitecoreSharedFieldConfiguration
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
