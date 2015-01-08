namespace Unic.Flex.Core.Utilities
{
    using System;

    /// <summary>
    /// Helper class for Type specific actions.
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        /// Determines whether a type is a subclass of a generic type.
        /// </summary>
        /// <param name="generic">The type we search for.</param>
        /// <param name="toCheck">The type we want to check.</param>
        /// <returns>Boolean value if cheked class is subtype of the generic base class</returns>
        public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var current = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == current)
                {
                    return true;
                }

                toCheck = toCheck.BaseType;
            }

            return false;
        }
    }
}
