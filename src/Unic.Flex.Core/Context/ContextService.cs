namespace Unic.Flex.Core.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Glass.Mapper;
    using Newtonsoft.Json.Linq;
    using Sitecore.Configuration;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Unic.Flex.Core.Logging;
    using Unic.Flex.Core.Mapping;
    using Unic.Flex.Model.Forms;
    using Unic.Flex.Model.Steps;
    using Unic.Flex.Model.Types;
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
        /// The logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextService" /> class.
        /// </summary>
        /// <param name="formRepository">The form repository.</param>
        /// <param name="userDataRepository">The user data repository.</param>
        /// <param name="logger">The logger.</param>
        public ContextService(IFormRepository formRepository, IUserDataRepository userDataRepository, ILogger logger)
        {
            this.formRepository = formRepository;
            this.userDataRepository = userDataRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Loads the form based on a datasource.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <param name="useVersionCountDisabler">if set to <c>true</c> the version count disabler is used to load the form.</param>
        /// <returns>
        /// The loaded form domain model object.
        /// </returns>
        public virtual IForm LoadForm(string dataSource, bool useVersionCountDisabler = false)
        {
            Profiler.OnStart(this, "Flex :: Load form from datasource");

            var form = this.formRepository.LoadForm(dataSource, useVersionCountDisabler);
            if (form == null)
            {
                this.logger.Warn(string.Format("Could not load form with datasource '{0}', maybe it's not available in all languages as the item containing the form", dataSource), this);
                Profiler.OnEnd(this, "Flex :: Load form from datasource");
                return null;
            }
            
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

            // reference the dependent fields
            foreach (var section in form.GetSections().Where(f => f.DependentField != null))
            {
                section.DependentField = form.GetField(section.DependentField);
            }

            foreach (var field in form.GetFields().Where(f => f.DependentField != null))
            {
                field.DependentField = form.GetField(field.DependentField);
            }

            Profiler.OnEnd(this, "Flex :: Load form from datasource");

            return form;
        }

        /// <summary>
        /// Populates the form values from the session into the form.
        /// </summary>
        /// <param name="form">The form domain model.</param>
        public virtual void PopulateFormValues(IForm form)
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

            Profiler.OnEnd(this, "Flex :: Populating values from user data storage");
        }

        /// <summary>
        /// Populates the form values from a dictionary.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="values">The values.</param>
        public virtual void PopulateFormValues(IForm form, IDictionary<string, object> values)
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

                        try
                        {
                            // deserialize JObject again, because if this is the type, then it's an uploaded file within it
                            var fileValue = value as JObject;
                            field.Value = fileValue != null ? fileValue.ToObject<UploadedFile>() : value;
                        }
                        catch (Exception exception)
                        {
                            this.logger.Warn(
                                string.Format("Could not map field '{0}' of form '{1}'. Ignoring field and go on...", field.Id, form.Id),
                                this,
                                exception);
                        }
                    }
                    else
                    {
                        field.Value = field.DefaultValue;
                    }
                }
            }

            Profiler.OnEnd(this, "Flex :: Populating values from dictionary");
        }

        /// <summary>
        /// Stores the form values into the session.
        /// </summary>
        /// <param name="form">The form.</param>
        public virtual void StoreFormValues(IForm form)
        {
            Assert.ArgumentNotNull(form, "form");

            Profiler.OnStart(this, "Flex :: Store form values to user data storage");

            foreach (var section in form.ActiveStep.Sections)
            {
                // check if the complete section is invisible -> remove all fields and go to next sections
                if (section.IsHidden)
                {
                    section.Fields.ForEach(f => this.userDataRepository.RemoveValue(form.Id, f.Id));
                    continue;
                }

                foreach (var field in section.Fields)
                {
                    // check if field is invisible -> remove from storage and go to next field
                    if (field.IsHidden)
                    {
                        this.userDataRepository.RemoveValue(form.Id, field.Id);
                        field.Value = field.Type.IsValueType ? Activator.CreateInstance(field.Type) : null;
                        continue;
                    }

                    this.userDataRepository.SetValue(form.Id, field.Id, field.Value);
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
        public virtual bool IsStepAccessible(IForm form, IStep step)
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
    }
}
