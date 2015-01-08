namespace Unic.Flex.Core.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sitecore.Diagnostics;
    using Unic.Flex.Model.DomainModel.Components;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.ViewModel.Fields;
    using Profiler = Unic.Profiling.Profiler;

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
            var dependentField = allFields.FirstOrDefault(f => f.Id == dependency.DependentFrom);
            if (dependentField == null || dependentField.Value == null) return false;

            var referencedListValue = dependentField.Value as IEnumerable<string>;
            var referencedValue = referencedListValue != null ? string.Join(",", referencedListValue) : dependentField.Value.ToString();
            return referencedValue.Equals(dependency.DependentValue, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Handles the visibility dependency for fields with a field dependency.
        /// </summary>
        /// <param name="form">The form.</param>
        public virtual void HandleVisibilityDependency(Form form)
        {
            Assert.ArgumentNotNull(form, "form");

            Profiler.OnEnd(this, "Flex :: Handle visibility dependency and mark invisible fields");

            // handle section and fields
            this.HandleHiddenFlag(form, form.GetSections().SelectMany(s => s.Fields).Where(f => f.DependentField != null));
            this.HandleHiddenFlag(form, form.GetSections().Where(s => s.DependentField != null));

            // mark sections with only hidden fields also as hidden
            foreach (var section in form.GetSections().Where(s => s.Fields.All(f => f.IsHidden)))
            {
                section.IsHidden = true;
            }

            Profiler.OnEnd(this, "Flex :: Handle visibility dependency and mark invisible fields");
        }

        /// <summary>
        /// Handles the hidden flag for components with a field dependency.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="components">The components.</param>
        private void HandleHiddenFlag(Form form, IEnumerable<IVisibilityDependency> components)
        {
            foreach (var component in components)
            {
                var dependentValue = form.GetFieldValue(component.DependentField);
                component.IsHidden = !dependentValue.Equals(component.DependentValue, StringComparison.InvariantCultureIgnoreCase);
            }
        }
    }
}
