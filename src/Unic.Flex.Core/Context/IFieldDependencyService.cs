namespace Unic.Flex.Core.Context
{
    using System.Collections.Generic;
    using Unic.Flex.Model.ViewModels.Components;
    using Unic.Flex.Model.ViewModels.Fields;

    /// <summary>
    /// Methods for handling field dependency
    /// </summary>
    public interface IFieldDependencyService
    {
        /// <summary>
        /// Determines whether the dependency for a specific component is valid and the component is visible due to the condition.
        /// </summary>
        /// <param name="allFields">All fields.</param>
        /// <param name="dependency">The dependency.</param>
        /// <returns>
        /// Boolean value if the condition is valid and the field is visible
        /// </returns>
        bool IsDependentFieldVisible(IEnumerable<IFieldViewModel> allFields, IVisibilityDependencyViewModel dependency);
    }
}
