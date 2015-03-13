namespace Unic.Flex.Core.Mapping
{
    using Unic.Flex.Core.Context;

    /// <summary>
    /// Interface for mapping properties needed for views
    /// </summary>
    public interface IViewMapper
    {
        /// <summary>
        /// Do a simple map with just needed properties. This is called form the model binder.
        /// </summary>
        /// <param name="context">The context.</param>
        void SimpleMap(IFlexContext context);

        /// <summary>
        /// Map all properties needed.
        /// </summary>
        /// <param name="context">The context.</param>
        void FullMap(IFlexContext context);
    }
}
