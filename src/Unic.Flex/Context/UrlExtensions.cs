namespace Unic.Flex.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Unic.Flex.Model.DomainModel;
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
        public static string GetUrl(this ItemBase item)
        {
            var context = FlexContext.Current;

            if (context.Item == null) return item.Url;
            if (context.Form == null) return item.Url;

            return string.Join("/", context.Item.Url, item.Url.Split('/').Last());
        }

        /// <summary>
        /// Gets the next step URL.
        /// </summary>
        /// <param name="step">The step.</param>
        /// <returns>Url of the next step if available</returns>
        public static string GetNextStepUrl(this StepBase step)
        {
            var node = GetCurrentStepNode(step).Next;
            return node != null ? node.Value.GetUrl() : string.Empty;
        }

        /// <summary>
        /// Gets the previous step URL.
        /// </summary>
        /// <param name="step">The step.</param>
        /// <returns>Url of the previous step if available</returns>
        public static string GetPreviousStepUrl(this StepBase step)
        {
            var node = GetCurrentStepNode(step).Previous;
            return node != null ? node.Value.GetUrl() : string.Empty;
        }

        /// <summary>
        /// Gets the current step node.
        /// </summary>
        /// <param name="step">The step.</param>
        /// <returns>The current step as linked list node</returns>
        /// <exception cref="System.Exception">Could not convert steps to linked list</exception>
        private static LinkedListNode<StepBase> GetCurrentStepNode(StepBase step)
        {
            var linkedSteps = new LinkedList<StepBase>(FlexContext.Current.Form.Steps);
            var stepNode = linkedSteps.Find(step);
            if (stepNode == null) throw new Exception("Could not convert steps to linked list");
            return stepNode;
        }
    }
}
