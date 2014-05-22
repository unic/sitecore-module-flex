namespace Unic.Flex.Model.ViewModel.Sections
{
    using System.Collections.Generic;
    using Unic.Flex.Model.DomainModel;
    using Unic.Flex.Model.Presentation;
    using Unic.Flex.Model.ViewModel.Fields;

    /// <summary>
    /// This view model covers a section in the form step.
    /// </summary>
    public class SectionViewModel : IPresentationComponent, IViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SectionViewModel"/> class.
        /// </summary>
        /// <param name="domainModel">The domain model.</param>
        public SectionViewModel(ItemBase domainModel)
        {
            this.Fields = new List<FieldViewModel>();
            this.DomainModel = domainModel;
        }

        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        public IList<FieldViewModel> Fields { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable the fieldset markup.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the fieldset markup should not be outputed; otherwise, <c>false</c>.
        /// </value>
        public bool DisableFieldset { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public string ViewName { get; set; }

        /// <summary>
        /// Gets or sets the domain model.
        /// </summary>
        /// <value>
        /// The domain model.
        /// </value>
        public ItemBase DomainModel { get; set; }
    }
}