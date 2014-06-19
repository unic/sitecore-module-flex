namespace Unic.Flex.Model.Configuration.Converters
{
    using Glass.Mapper.Sc;
    using Sitecore.Diagnostics;
    using Unic.Configuration;
    using Unic.Configuration.Converter;
    using Unic.Flex.Model.DomainModel.Global;

    /// <summary>
    /// Converter for specification items.
    /// </summary>
    public class SpecificationConverter : AbstractConverter<Specification>
    {
        /// <summary>
        /// Converts the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public override Specification Convert(IConfigurationField field)
        {
            Assert.ArgumentNotNull(field, "field");

            using (new VersionCountDisabler())
            {
                return string.IsNullOrWhiteSpace(field.Value) ? null : (new SitecoreContext()).GetItem<Specification>(field.Value);
            }
        }
    }
}
