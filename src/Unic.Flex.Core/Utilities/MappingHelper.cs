namespace Unic.Flex.Core.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Utilities for mapping stuff.
    /// </summary>
    public static class MappingHelper
    {
        /// <summary>
        /// The replace collection method
        /// </summary>
        private static readonly MethodInfo ReplaceCollectionMethod = typeof(MappingHelper).GetMethod("ReplaceCollection", BindingFlags.Static | BindingFlags.NonPublic);
        
        /// <summary>
        /// Gets the id of a html input field.
        /// </summary>
        /// <param name="sectionIndex">Index of the section.</param>
        /// <param name="fieldIndex">Index of the field.</param>
        /// <returns>Id string</returns>
        public static string GetFormFieldId(int sectionIndex, int fieldIndex)
        {
            return string.Format("ActiveStep.Sections[{0}].Fields[{1}].Value", sectionIndex, fieldIndex);
        }

        /// <summary>
        /// Replaces a collection with new content.
        /// </summary>
        /// <param name="collectionType">Type of the collection.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="newCollection">The new collection.</param>
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void ReplaceCollection(Type collectionType, object collection, object newCollection)
        {
            ReplaceCollectionMethod.MakeGenericMethod(collectionType).Invoke(null, new[] { collection, newCollection });
        }

        /// <summary>
        /// Replaces a collection with new content.
        /// </summary>
        /// <typeparam name="T">Type of the collection</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="newCollection">The new collection.</param>
        private static void ReplaceCollection<T>(ICollection<T> collection, IEnumerable newCollection)
        {
            collection.Clear();
            if (newCollection == null) return;

            foreach (var item in newCollection)
            {
                T newItem = item is T ? (T)item : default(T);
                collection.Add(newItem);
            }
        }
    }
}
