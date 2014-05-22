namespace Unic.Flex.Model.DomainModel.Steps
{
    using Glass.Mapper.Sc.Configuration.Attributes;

    /// <summary>
    /// A step in a multi step form.
    /// </summary>
    [SitecoreType(TemplateId = "{93EC9DA3-81FC-4A1E-82F8-DD988A2D355B}")]
    public class MultiStep : StandardStep
    {
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
