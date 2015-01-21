namespace Unic.Flex.Model.SortOrder
{
    using System;
    using Sitecore.Diagnostics;

    /// <summary>
    /// Sort order strategies.
    /// </summary>
    public static class StrategyFactory
    {
        /// <summary>
        /// Creates an instance of a sort strategy.
        /// </summary>
        /// <param name="fullType">The full type.</param>
        /// <returns>Instance of type</returns>
        public static ISortOrderStrategy CreateInstance(string fullType)
        {
            Assert.ArgumentNotNullOrEmpty(fullType, "fullType");

            var type = Type.GetType(fullType);
            if (type == null) return null;

            return Activator.CreateInstance(type) as ISortOrderStrategy;
        }
    }
}
