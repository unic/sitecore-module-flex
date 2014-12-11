namespace Unic.Flex.Utilities
{
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Data.Managers;

    /// <summary>
    /// Item helper methods
    /// </summary>
    public static class ItemHelpers
    {
        /// <summary>
        /// Determines whether the specified item is based on or inherited from the given template
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="baseTemplate">The base template.</param>
        /// <returns>The check result</returns>
        public static bool HasBaseTemplate(this Item item, string baseTemplate)
        {
            if (item == null || string.IsNullOrWhiteSpace(baseTemplate)) return false;

            var template = TemplateManager.GetTemplate(item);
            if (template == null) return false;

            if (ID.IsID(baseTemplate) || ShortID.IsShortID(baseTemplate))
            {
                return template.InheritsFrom(new ID(baseTemplate));
            }

            return template.InheritsFrom(baseTemplate);
        }
    }
}
