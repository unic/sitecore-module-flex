namespace Unic.Flex.Model.Configuration.Extensions
{
    using System;
    using System.Linq.Expressions;
    using Unic.Configuration;
    using Unic.Flex.Model.Specifications;

    /// <summary>
    /// The configuration manager extensions.
    /// </summary>
    public static class ConfigurationManagerExtensions
    {
        /// <summary>
        /// Gets the specified configuration value.
        /// </summary>
        /// <typeparam name="TType">The type of the configuration.</typeparam>
        /// <param name="configuration">The configuration manager.</param>
        /// <param name="func">The property.</param>
        /// <returns>The configuration value.</returns>
        public static Specification Get<TType>(this IConfigurationManager configuration, Expression<Func<TType, Specification>> func) where TType : class
        {
            return configuration.Get(func);
        }
    }
}
