namespace Unic.Flex.Model.Forms
{
    using Unic.Flex.Model.GlassExtensions.Attributes;

    /// <summary>
    /// The model for form statistics
    /// </summary>
    public class StatisticForm
    {
        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        [SitecoreSharedQuery("./ancestor::*[@@templateid='{7212DD21-1769-43AA-AF6A-92756C97A269}']", IsRelative = true, InferType = true)]
        public virtual Repository Repository { get; set; }
    }
}
