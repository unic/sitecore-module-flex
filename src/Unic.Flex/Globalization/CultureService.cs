namespace Unic.Flex.Globalization
{
    using System.Threading;
    using Unic.Configuration;
    using Unic.Flex.Model.Configuration;

    /// <summary>
    /// Culture depending helpers
    /// </summary>
    public class CultureService : ICultureService
    {
        /// <summary>
        /// The configuration manager
        /// </summary>
        private readonly IConfigurationManager configurationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureService"/> class.
        /// </summary>
        /// <param name="configurationManager">The configuration manager.</param>
        public CultureService(IConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;
        }
        
        /// <summary>
        /// Gets the current date format.
        /// </summary>
        /// <returns>
        /// Date format
        /// </returns>
        public string GetDateFormat()
        {
            var format = this.configurationManager.Get<GlobalizationConfiguration>(c => c.DateFormat);
            if (string.IsNullOrWhiteSpace(format))
            {
                format = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
            }

            return format;
        }
    }
}
