namespace Unic.Flex.Core.Mapping
{
    using Unic.Flex.Core.Context;

    /// <summary>
    /// Interface for mapping properties needed for views
    /// </summary>
    public interface IViewMapper
    {
        /// <summary>
        /// Map all properties needed.
        /// </summary>
        /// <param name="context">The context.</param>
        void Map(IFlexContext context);
    }
}
