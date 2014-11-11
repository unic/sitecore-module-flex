namespace Unic.Flex.Context
{
    using Castle.Core.Internal;
    using Newtonsoft.Json.Linq;
    using Sitecore.Configuration;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Unic.Flex.Mapping;
    using Unic.Flex.Model.DomainModel.Components;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Steps;
    using Unic.Flex.Model.Types;
    using Unic.Flex.Model.ViewModel.Fields;
    using Unic.Flex.Model.ViewModel.Forms;
    using Profiler = Unic.Profiling.Profiler;

    /// <summary>
    /// The service containing contexxt based business logic.
    /// </summary>
    public class ContextService : IContextService
    {
        /// <summary>
        /// The form repository
        /// </summary>
        private readonly IFormRepository formRepository;

        /// <summary>
        /// The user data repository
        /// </summary>
        private readonly IUserDataRepository userDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextService"/> class.
        /// </summary>
        /// <param name="formRepository">The form repository.</param>
        /// <param name="userDataRepository">The user data repository.</param>
        public ContextService(IFormRepository formRepository, IUserDataRepository userDataRepository)
        {
            this.formRepository = formRepository;
            this.userDataRepository = userDataRepository;
        }

        /// <summary>
        /// Loads the form based on a datasource.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <returns>
        /// The loaded form domain model object.
        /// </returns>
        public virtual Form LoadForm(string dataSource)
        {
            Profiler.OnStart(this, "Flex :: Load form from datasource");
            
            var form = this.formRepository.LoadForm(dataSource);
            
            // set the step number for each step
            var counter = 0;
            foreach (var step in form.Steps)
            {
                step.StepNumber = ++counter;
            }

            // remove skippable flag from the last step
            var lastStep = form.Steps.LastOrDefault() as MultiStep;
            if (lastStep != null)
            {
                lastStep.IsSkippable = false;
                lastStep.IsLastStep = true;
            }

            Profiler.OnEnd(this, "Flex :: Load form from datasource");

            return form;
        }

        /// <summary>
        /// Populates the form values from the session into the form.
        /// </summary>
        /// <param name="form">The form domain model.</param>
        public virtual void PopulateFormValues(Form form)
        {
            Assert.ArgumentNotNull(form, "form");

            Profiler.OnStart(this, "Flex :: Populating values from user data storage");

            foreach (var section in form.Steps.SelectMany(step => step.Sections))
            {
                foreach (var field in section.Fields)
                {
                    field.Value = this.userDataRepository.IsFieldStored(form.Id, field.Id)
                                      ? this.userDataRepository.GetValue(form.Id, field.Id)
                                      : field.DefaultValue;
                }
            }

            // set properties for hidden fields
            this.HandleVisibilityDependency(form);

            Profiler.OnEnd(this, "Flex :: Populating values from user data storage");
        }

        /// <summary>
        /// Populates the form values from a dictionary.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="values">The values.</param>
        public virtual void PopulateFormValues(Form form, IDictionary<string, object> values)
        {
            Assert.ArgumentNotNull(form, "form");

            Profiler.OnStart(this, "Flex :: Populating values from dictionary");

            foreach (var section in form.Steps.SelectMany(step => step.Sections))
            {
                foreach (var field in section.Fields)
                {
                    if (values.ContainsKey(field.Id))
                    {
                        var value = values[field.Id];

                        // deserialize JObject again, because if this is the type, then it's an uploaded file within it
                        var fileValue = value as JObject;
                        field.Value = fileValue != null ? fileValue.ToObject<UploadedFile>() : value;
                    }
                    else
                    {
                        field.Value = field.DefaultValue;
                    }
                }
            }

            // set properties for hidden fields
            this.HandleVisibilityDependency(form);

            Profiler.OnEnd(this, "Flex :: Populating values from dictionary");
        }

        /// <summary>
        /// Stores the form values into the session.
        /// </summary>
        /// <param name="viewModel">The form view model containing the current values.</param>
        public virtual void StoreFormValues(IFormViewModel viewModel)
        {
            Assert.ArgumentNotNull(viewModel, "viewModel");

            Profiler.OnStart(this, "Flex :: Store form values to user data storage");

            var allFields = viewModel.Step.Sections.SelectMany(section => section.Fields).ToList();
            foreach (var section in viewModel.Step.Sections)
            {
                // check if the complete section is invisible -> remove all fields and go to next sections
                if (!string.IsNullOrWhiteSpace(section.DependentFrom) && !this.IsDependencyVisible(allFields, section.DependentFrom, section.DependentValue))
                {
                    section.Fields.ForEach(f => this.userDataRepository.RemoveValue(viewModel.Id, f.Id));
                    continue;
                }

                foreach (var field in section.Fields)
                {
                    // check if field is invisible -> remove from storage and go to next field
                    if (!string.IsNullOrWhiteSpace(field.DependentFrom) && !this.IsDependencyVisible(allFields, field.DependentFrom, field.DependentValue))
                    {
                        this.userDataRepository.RemoveValue(viewModel.Id, field.Id);
                        continue;
                    }

                    this.userDataRepository.SetValue(viewModel.Id, field.Id, field.Value);
                }
            }

            Profiler.OnEnd(this, "Flex :: Store form values to user data storage");
        }

        /// <summary>
        /// Determines whether the given step can be actually accessed. This is only valid if all previous steps has been processed.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="step">The step.</param>
        /// <returns>
        /// Boolean value if the step may accessed by the user or not
        /// </returns>
        public virtual bool IsStepAccessible(Form form, StepBase step)
        {
            Assert.ArgumentNotNull(form, "form");
            Assert.ArgumentNotNull(step, "step");

            // return for simple cases
            if (!form.Steps.Any()) return false;
            if (step.StepNumber == 1) return true;

            // get the last step which is not skippable
            var lastNeededStep = form.Steps.OfType<MultiStep>()
                .Where(multiStep => multiStep.StepNumber < step.StepNumber)
                .LastOrDefault(multiStep => !multiStep.IsSkippable);

            // check if step has been completed
            return lastNeededStep == null || this.userDataRepository.IsStepCompleted(form.Id, lastNeededStep.StepNumber);
        }

        /// <summary>
        /// Gets the rendering datasource of a form.
        /// </summary>
        /// <param name="item">The item to search for a referenced form.</param>
        /// <param name="device">The device.</param>
        /// <returns>
        /// Datasource/form id if form is included on item
        /// </returns>
        public virtual string GetRenderingDatasource(Item item, DeviceItem device)
        {
            Assert.ArgumentNotNull(item, "item");
            Assert.ArgumentNotNull(device, "device");
            
            var renderingId = new ID(Settings.GetSetting("Flex.RenderingId"));
            var renderingReferences = item.Visualization.GetRenderings(device, false);
            var rendering = renderingReferences.FirstOrDefault(reference => reference.RenderingID == renderingId);
            return rendering == null ? string.Empty : rendering.Settings.DataSource;
        }

        /// <summary>
        /// Determines whether the dependency for a specific component is valid and the component is visible due to the condition.
        /// </summary>
        /// <param name="allFields">All fields.</param>
        /// <param name="dependentFrom">The dependent from.</param>
        /// <param name="dependentValue">The dependent value.</param>
        /// <returns>Boolean value if the condition is valid and the field is visible</returns>
        protected virtual bool IsDependencyVisible(IEnumerable<IFieldViewModel> allFields, string dependentFrom, string dependentValue)
        {
            var dependentField = allFields.FirstOrDefault(f => f.Id == dependentFrom);
            return dependentField != null && dependentField.Value != null && dependentField.Value.ToString().Equals(dependentValue, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Handles the visibility dependency for fields with a field dependency.
        /// </summary>
        /// <param name="form">The form.</param>
        protected virtual void HandleVisibilityDependency(Form form)
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
