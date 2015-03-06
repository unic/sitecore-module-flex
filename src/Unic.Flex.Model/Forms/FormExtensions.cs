﻿namespace Unic.Flex.Model.Forms
{
    using System.Collections.Generic;
    using System.Linq;
    using Unic.Flex.Model.DomainModel.Fields;
    using Unic.Flex.Model.DomainModel.Sections;
    using Unic.Flex.Model.Steps;

    /// <summary>
    /// Extension methods for a form.
    /// </summary>
    public static class FormExtensions
    {
        /// <summary>
        /// Gets all the real sections from a form.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="stepNumber">The step number.</param>
        /// <returns>
        /// All real sections, for all steps or only for one
        /// </returns>
        public static IEnumerable<StandardSection> GetSections(this Form form, int stepNumber = 0)
        {
            if (form == null) return Enumerable.Empty<StandardSection>();
            
            var steps = form.Steps;
            if (stepNumber > 0)
            {
                steps = steps.Where(step => step.StepNumber == stepNumber);
            }

            return steps.Where(step => !(step is Summary)).SelectMany(s => s.Sections);
        }

        /// <summary>
        /// Gets all the fields from a form.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="stepNumber">The step number.</param>
        /// <returns>
        /// List of fields
        /// </returns>
        public static IEnumerable<IField> GetFields(this Form form, int stepNumber = 0)
        {
            return form != null ? form.GetSections(stepNumber).SelectMany(s => s.Fields) : Enumerable.Empty<IField>();
        }

        /// <summary>
        /// Gets the field from the current form. This is used because a referenced field not always have mapped values.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="field">The field.</param>
        /// <returns>
        /// The field mapped by the form
        /// </returns>
        public static IField GetField(this Form form, IField field)
        {
            return form == null || field == null ? null : form.GetFields().FirstOrDefault(f => f.ItemId == field.ItemId);
        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="field">The field.</param>
        /// <returns>
        /// Value of the field in the form
        /// </returns>
        public static string GetFieldValue(this Form form, IField field)
        {
            if (form == null || field == null) return string.Empty;
            var formField = form.GetField(field);
            if (formField == null || formField.Value == null) return string.Empty;

            var listValue = formField.Value as IEnumerable<string>;
            return listValue != null ? string.Join(",", listValue) : formField.Value.ToString();
        }
    }
}
