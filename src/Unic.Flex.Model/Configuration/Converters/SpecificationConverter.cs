namespace Unic.Flex.Model.Configuration.Converters
{
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
        /// <returns>The specification model</returns>
        public override Specification Convert(IConfigurationField field)
        {
            Assert.ArgumentNotNull(field, "field");

            if (string.IsNullOrWhiteSpace(field.Value)) return null;

            var item = field.Item.Database.GetItem(field.Value);
            return item == null ? null : new Specification { Value = item["Value"] };
        }
    }
}
