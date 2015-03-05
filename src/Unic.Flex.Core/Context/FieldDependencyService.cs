namespace Unic.Flex.Core.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Unic.Flex.Model.ViewModel.Components;
    using Unic.Flex.Model.ViewModel.Fields;

    /// <summary>
    /// Methods for handling field dependency
    /// </summary>
    public class FieldDependencyService : IFieldDependencyService
    {
        /// <summary>
        /// Determines whether the dependency for a specific component is valid and the component is visible due to the condition.
        /// </summary>
        /// <param name="allFields">All fields.</param>
        /// <param name="dependency">The dependency.</param>
        /// <returns>
        /// Boolean value if the condition is valid and the field is visible
        /// </returns>
        public virtual bool IsDependentFieldVisible(IEnumerable<IFieldViewModel> allFields, IVisibilityDependencyViewModel dependency)
        {
            var dependentField = allFields.FirstOrDefault(f => f.Id == dependency.DependentFieldId);
            if (dependentField == null || dependentField.Value == null) return false;

            var referencedListValue = dependentField.Value as IEnumerable<string>;
            var referencedValue = referencedListValue != null ? string.Join(",", referencedListValue) : dependentField.Value.ToString();
            return referencedValue.Equals(dependency.DependentValue, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
