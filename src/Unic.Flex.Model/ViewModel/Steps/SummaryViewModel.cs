namespace Unic.Flex.Model.ViewModel.Steps
{
    using System;
    using System.Collections.Generic;
    using Unic.Flex.Model.ViewModel.Sections;

    /// <summary>
    /// View model for a summary step
    /// </summary>
    public class SummaryViewModel : StepBaseViewModel
    {
        /// <summary>
        /// Gets the sections.
        /// </summary>
        /// <value>
        /// The sections.
        /// </value>
        /// <exception cref="System.NotSupportedException">Summary does not section viewmodels</exception>
        public override IList<StandardSectionViewModel> Sections
        {
            get
            {
                throw new NotSupportedException("Summary does not section viewmodels");
            }
        }
    }
}
