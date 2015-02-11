namespace Unic.Flex.Core.Tests.Utilities
{
    using System;
    using NUnit.Framework;
    using Unic.Flex.Core.Utilities;
    using Unic.Flex.Implementation.Fields.InputFields;
    using Unic.Flex.Implementation.Fields.ListFields;
    using Unic.Flex.Model.DomainModel;
    using Unic.Flex.Model.DomainModel.Fields.ListFields;

    /// <summary>
    /// Tests for type helper.
    /// </summary>
    public class TypeHelperTests
    {
        /// <summary>
        /// Tests for the method to check if a class is subtype of a generic other class.
        /// </summary>
        public class TheIsSubclassOfRawGenericMethod
        {
            /// <summary>
            /// Tests if the given types are correctly compared to it's subtypes.
            /// </summary>
            /// <param name="generic">The generic.</param>
            /// <param name="toCheck">To check.</param>
            /// <returns>Value of the check.</returns>
            [TestCase(typeof(ListField<,>), typeof(CheckBoxListField), Result = true)]
            [TestCase(typeof(ListField<,>), typeof(SinglelineTextField), Result = false)]
            [TestCase(typeof(ItemBase), typeof(SinglelineTextField), Result = true)]
            [TestCase(typeof(object), typeof(SinglelineTextField), Result = false)]
            [TestCase(null, null, Result = false)]
            public bool WillCorrectlyCompareTypes(Type generic, Type toCheck)
            {
                return TypeHelper.IsSubclassOfRawGeneric(generic, toCheck);
            }
        }
    }
}
