namespace Unic.Flex.Core.Utilities
{
    /// <summary>
    /// Utilities for mapping stuff.
    /// </summary>
    public static class MappingHelper
    {
        /// <summary>
        /// Gets the id of a html input field.
        /// </summary>
        /// <param name="sectionIndex">Index of the section.</param>
        /// <param name="fieldIndex">Index of the field.</param>
        /// <returns>Id string</returns>
        public static string GetFormFieldId(int sectionIndex, int fieldIndex)
        {
            return string.Format("Step.Sections[{0}].Fields[{1}].Value", sectionIndex, fieldIndex);
        }
    }
}
