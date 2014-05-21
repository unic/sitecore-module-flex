namespace Unic.Flex.Model.GlassExtensions.Configurations
{
    using Glass.Mapper.Sc.Configuration;

    /// <summary>
    /// Configuration for the SitecoreDictionaryFallbackField
    /// </summary>
    public class SitecoreDictionaryFallbackFieldConfiguration : SitecoreFieldConfiguration
    {
        /// <summary>
        /// Gets or sets the dictionary key.
        /// </summary>
        /// <value>
        /// The dictionary key.
        /// </value>
        public string DictionaryKey { get; set; }

        /// <summary>
        /// Makes a copy of the SitecoreFieldConfiguration
        /// </summary>
        /// <returns>The copied config of specific type.</returns>
        public override SitecoreFieldConfiguration Copy()
        {
            return new SitecoreDictionaryFallbackFieldConfiguration
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