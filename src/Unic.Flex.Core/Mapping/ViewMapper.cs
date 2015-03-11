namespace Unic.Flex.Core.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Sitecore.Diagnostics;
    using Unic.Configuration;
    using Unic.Flex.Core.Context;
    using Unic.Flex.Core.Definitions;
    using Unic.Flex.Core.Globalization;
    using Unic.Flex.Model.Components;
    using Unic.Flex.Model.Forms;
    using Unic.Flex.Model.Sections;
    using Unic.Flex.Model.Steps;
    using Unic.Flex.Model.ViewModel.Components;
    using NavigationItem = Unic.Flex.Model.Components.NavigationItem;
    using Unic.Flex.Model.Fields;
    using Unic.Flex.Model.Fields.ListFields;
    using Unic.Flex.Model.Types;
    using Unic.Flex.Model.Validation;
    using Unic.Flex.Core.Utilities;
    using Unic.Flex.Model.Configuration;

    public class ViewMapper : IViewMapper
    {
        private readonly IUrlService urlService;

        private readonly IDictionaryRepository dictionaryRepository;

        private readonly IConfigurationManager configurationManager;

        private string optionalLabelText;

        public ViewMapper(IUrlService urlService, IDictionaryRepository dictionaryRepository, IConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;
            this.dictionaryRepository = dictionaryRepository;
            this.urlService = urlService;
        }

        public virtual void MapActiveStep(IFlexContext context)
        {
            if (context == null || context.Form == null || context.Form.ActiveStep == null) return;

            Profiling.Profiler.OnStart(this, "Flex :: Map active step for view");

            // get config
            this.optionalLabelText = this.configurationManager.Get<GlobalConfiguration>(c => c.OptionalFieldsLabelText);
            
            // map the step
            this.MapStep(context);

            // map the sections
            foreach (var section in context.Form.ActiveStep.Sections)
            {
                this.MapSection(section);

                foreach (var field in section.Fields)
                {
                    this.MapField(field);
                }
            }

            Profiling.Profiler.OnEnd(this, "Flex :: Map active step for view");
        }

        protected virtual void MapStep(IFlexContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            
            // get the form
            var form = context.Form;
            Assert.IsNotNull(form, "form must not be null");
            
            // get linked steps
            var linkedSteps = new LinkedList<StepBase>(form.Steps);
            var currentStep = linkedSteps.Find(form.ActiveStep);
            if (currentStep == null) throw new Exception("Could not found current step in linked step list");
            
            // handle multistep
            var multiStep = form.ActiveStep as MultiStep;
            if (multiStep != null)
            {
                multiStep.PreviousStepUrl = currentStep.Previous != null ? currentStep.Previous.Value.GetUrl(context) : string.Empty;
                multiStep.NextStepUrl = currentStep.Next != null ? currentStep.Next.Value.GetUrl(context) : string.Empty;
                if (multiStep.ShowNavigationPane) multiStep.NavigationPane = this.GetNavigationPane(context);
            }

            // handle summary
            var summary = form.ActiveStep as Summary;
            if (summary != null)
            {
                var allSections = form.Steps.SelectMany(s => s.Sections).ToList();
                summary.LazySections = allSections;
                summary.Sections = allSections;
                summary.PreviousStepUrl = currentStep.Previous != null ? currentStep.Previous.Value.GetUrl(context) : string.Empty;
                if (summary.ShowNavigationPane) summary.NavigationPane = this.GetNavigationPane(context);
            }

            // add honeypot field
            if (summary == null)
            {
                // todo: add honeypot field
            }

            // handle cancel link
            if (form.CancelLink != null && !string.IsNullOrWhiteSpace(form.CancelLink.Url))
            {
                form.ActiveStep.CancelUrl = this.urlService.AddQueryStringToCurrentUrl(
                    Constants.CancelQueryStringKey,
                    Constants.CancelQueryStringValue);

                form.ActiveStep.CancelText = !string.IsNullOrWhiteSpace(form.CancelLink.Text)
                                 ? form.CancelLink.Text
                                 : this.dictionaryRepository.GetText("Cancel Form");
            }
        }

        protected virtual void MapSection(StandardSection section)
        {
            // map the tooltip
            if (!string.IsNullOrWhiteSpace(section.TooltipTitle) && !string.IsNullOrWhiteSpace(section.TooltipText))
            {
                section.Tooltip = this.GetTooltip(section.TooltipTitle, section.TooltipText);
            }

            // map properties
            section.BindProperties();
        }

        protected virtual void MapField(IField field)
        {
            // add required validator
            if (field.IsRequired)
            {
                if (TypeHelper.IsSubclassOfRawGeneric(typeof(MulticheckListField<,>), field.GetType()))
                {
                    field.AddValidator(new MulticheckRequired { ValidationMessage = field.ValidationMessage });
                }
                else if (field.Type == typeof(UploadedFile))
                {
                    field.AddValidator(new FileRequiredValidator { ValidationMessage = field.ValidationMessage });
                }
                else
                {
                    field.AddValidator(new RequiredValidator { ValidationMessage = field.ValidationMessage });
                }
            }
            else if (!string.IsNullOrWhiteSpace(this.optionalLabelText))
            {
                field.LabelAddition = this.optionalLabelText;
            }

            // add all other validators
            foreach (var validator in field.Validators.Concat(field.DefaultValidators))
            {
                if (string.IsNullOrWhiteSpace(validator.ValidationMessage))
                {
                    validator.ValidationMessage = this.dictionaryRepository.GetText(validator.DefaultValidationMessageDictionaryKey);
                }

                field.AddValidator(validator);
            }

            // add css classes
            if (field.CustomCssClass != null && !string.IsNullOrWhiteSpace(field.CustomCssClass.Value))
            {
                field.AddCssClass(field.CustomCssClass.Value);
            }

            if (!string.IsNullOrWhiteSpace(field.AdditionalCssClass))
            {
                field.AddCssClass(field.AdditionalCssClass);
            }

            // map the tooltip
            if (!string.IsNullOrWhiteSpace(field.TooltipTitle) && !string.IsNullOrWhiteSpace(field.TooltipText))
            {
                field.Tooltip = this.GetTooltip(field.TooltipTitle, field.TooltipText);
            }

            // bind properties
            field.BindProperties();
        }

        /// <summary>
        /// Gets the navigation pane.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// Navigation pane with navigation items
        /// </returns>
        protected virtual NavigationPane GetNavigationPane(IFlexContext context)
        {
            Assert.ArgumentNotNull(context, "context");

            // get the form
            var form = context.Form;
            Assert.IsNotNull(form, "form must not be null");

            var currentStep = form.ActiveStep.StepNumber;
            var navigationPane = new NavigationPane();
            foreach (var step in context.Form.Steps)
            {
                navigationPane.Items.Add(new NavigationItem
                {
                    IsActive = step.StepNumber == currentStep,
                    IsLinked = step.StepNumber < currentStep,
                    Title = step.Title,
                    Url = step.GetUrl(context)
                });
            }

            return navigationPane;
        }

        /// <summary>
        /// Gets the tooltip based on title and text.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <returns>Tooltip instance</returns>
        protected virtual Tooltip GetTooltip(string title, string text)
        {
            Assert.ArgumentNotNullOrEmpty(title, "title");
            Assert.ArgumentNotNullOrEmpty(text, "text");

            return new Tooltip { Title = title, Text = text };
        }
    }
}
