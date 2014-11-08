namespace Unic.Flex.Model.ViewModel.Sections
{
    using System.Collections.Generic;
    using System.Web;
    using Unic.Flex.Model.ViewModel.Components;
    using Unic.Flex.Model.ViewModel.Fields;

    /// <summary>
    /// This view model covers a section in the form step.
    /// </summary>
    public class StandardSectionViewModel : ISectionViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardSectionViewModel" /> class.
        /// </summary>
        public StandardSectionViewModel()
        {
            this.Fields = new List<IFieldViewModel>();
            this.ContainerAttributes = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        public virtual IList<IFieldViewModel> Fields { get; private set; }

        /// <summary>
        /// Gets the container attributes.
        /// </summary>
        /// <value>
        /// The container attributes.
        /// </value>
        public virtual IDictionary<string, object> ContainerAttributes { get; private set; }

        /// <summary>
        /// Gets or sets the dependent field.
        /// </summary>
        /// <value>
        /// The dependent field.
        /// </value>
        public virtual string DependentFrom { get; set; }

        /// <summary>
        /// Gets or sets the dependent value.
        /// </summary>
        /// <value>
        /// The dependent value.
        /// </value>
        public virtual string DependentValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable the fieldset markup.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the fieldset markup should not be outputed; otherwise, <c>false</c>.
        /// </value>
        public virtual bool DisableFieldset { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets the step title.
        /// </summary>
        /// <value>
        /// The step title.
        /// </value>
        public virtual string StepTitle { get; set; }

        /// <summary>
        /// Gets or sets the tooltip.
        /// </summary>
        /// <value>
        /// The tooltip.
        /// </value>
        public virtual TooltipViewModel Tooltip { get; set; }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public virtual string ViewName
        {
            get
            {
                return "Sections/StandardSection";
            }
        }

        /// <summary>
        /// Binds the needed attributes and properties after converting from domain model to the view model
        /// </summary>
        public virtual void BindProperties()
        {
            // handle field dependency
            if (!string.IsNullOrWhiteSpace(this.DependentFrom))
            {
                this.ContainerAttributes.Add("data-flexform-dependent", "{" + HttpUtility.HtmlEncode(string.Format("\"from\": \"{0}\", \"value\": \"{1}\"", this.DependentFrom, this.DependentValue)) + "}");
            }
        }
    }
}