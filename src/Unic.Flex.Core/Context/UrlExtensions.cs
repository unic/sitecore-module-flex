namespace Unic.Flex.Core.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Unic.Flex.Core.DependencyInjection;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Steps;

    /// <summary>
    /// Extensions for url related objects.
    /// </summary>
    public static class UrlExtensions
    {
        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>Url of the current item appended to the current form context</returns>
        public static string GetUrl(this StepBase item)
        {
            var context = Container.Resolve<IFlexContext>();

            if (context.Item == null) return item.Url;
            if (context.Form == null) return item.Url;
            if (item.StepNumber == 1) return context.Item.Url;

            return string.Join("/", context.Item.Url, item.Url.Split('/').Last());
        }

        /// <summary>
        /// Gets the next step URL.
        /// </summary>
        /// <param name="step">The step.</param>
        /// <returns>Url of the next step if available</returns>
        public static string GetNextStepUrl(this StepBase step)
        {
            var linkedSteps = new LinkedList<StepBase>(Container.Resolve<IFlexContext>().Form.Steps);
            var stepNode = linkedSteps.Find(step);
            if (stepNode == null) throw new Exception("Could not convert steps to linked list");
            var nextStep = stepNode.Next;
            return nextStep != null ? nextStep.Value.GetUrl() : string.Empty;
        }

        /// <summary>
        /// Gets the previous step URL.
        /// </summary>
        /// <param name="step">The step.</param>
        /// <returns>Url of the previous step if available</returns>
        public static string GetPreviousStepUrl(this StepBase step)
        {
            var context = Container.Resolve<IFlexContext>();
            var linkedSteps = new LinkedList<StepBase>(context.Form.Steps);
            var stepNode = linkedSteps.Find(step);
            if (stepNode == null) throw new Exception("Could not convert steps to linked list");
            var previousStep = stepNode.Previous;

            if (previousStep == null) return string.Empty;
            return previousStep.Equals(linkedSteps.First) ? context.Item.Url : previousStep.Value.GetUrl();
        }

        /// <summary>
        /// Gets the first step URL.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns>The url of the first step in the form</returns>
        public static string GetFirstStepUrl(this Form form)
        {
            var context = Container.Resolve<IFlexContext>();
            return context.Item == null ? string.Empty : context.Item.Url;
        }
    }
}
