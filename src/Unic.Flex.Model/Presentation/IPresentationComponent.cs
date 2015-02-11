namespace Unic.Flex.Model.Presentation
{
    /// <summary>
    /// Interface for a presentation component, which can be rendered by ASP.NET MVC:
    /// </summary>
    public interface IPresentationComponent
    {
        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        string ViewName { get; }
    }
}
