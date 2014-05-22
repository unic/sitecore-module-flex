﻿namespace Unic.Flex.Model.ViewModel.Steps
{
    using System.Collections.Generic;
    using Unic.Flex.Model.DomainModel;
    using Unic.Flex.Model.Presentation;
    using Unic.Flex.Model.ViewModel.Sections;

    /// <summary>
    /// This view model covers current step in the form.
    /// </summary>
    public class StepViewModel : IPresentationComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StepViewModel"/> class.
        /// </summary>
        /// <param name="domainModel">The domain model.</param>
        public StepViewModel(ItemBase domainModel)
        {
            this.Sections = new List<SectionViewModel>();
            this.DomainModel = domainModel;
        }

        /// <summary>
        /// Gets the sections.
        /// </summary>
        /// <value>
        /// The sections.
        /// </value>
        public IList<SectionViewModel> Sections { get; private set; }

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