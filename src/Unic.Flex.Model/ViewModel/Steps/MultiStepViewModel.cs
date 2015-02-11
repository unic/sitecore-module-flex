﻿namespace Unic.Flex.Model.ViewModel.Steps
{
    using Unic.Flex.Model.ViewModel.Components;

    /// <summary>
    /// View model for a step in a multi step form
    /// </summary>
    public class MultiStepViewModel : StepBaseViewModel, INavigationPaneViewModel
    {
        /// <summary>
        /// Gets or sets the next step URL.
        /// </summary>
        /// <value>
        /// The next step URL.
        /// </value>
        public virtual string NextStepUrl { get; set; }

        /// <summary>
        /// Gets or sets the previous step URL.
        /// </summary>
        /// <value>
        /// The previous step URL.
        /// </value>
        public virtual string PreviousStepUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the navigation pane.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the navigation pane should be shown; otherwise, <c>false</c>.
        /// </value>
        public virtual bool ShowNavigationPane { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this step is skippable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this step is skippable; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsSkippable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this step is the last step.
        /// </summary>
        /// <value>
        /// <c>true</c> if this step is the last step; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsLastStep { get; set; }

        /// <summary>
        /// Gets or sets the previous button text.
        /// </summary>
        /// <value>
        /// The previous button text.
        /// </value>
        public virtual string PreviousButtonText { get; set; }

        /// <summary>
        /// Gets or sets the skip button text.
        /// </summary>
        /// <value>
        /// The skip button text.
        /// </value>
        public virtual string SkipButtonText { get; set; }

        /// <summary>
        /// Gets or sets the navigation pane.
        /// </summary>
        /// <value>
        /// The navigation pane.
        /// </value>
        public virtual NavigationPaneViewModel NavigationPane { get; set; }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public override string ViewName
        {
            get
            {
                return "Steps/MultiStep";
            }
        }
    }
}
