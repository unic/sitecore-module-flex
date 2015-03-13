namespace Unic.Flex.Model.Steps
{
    using Glass.Mapper.Sc.Configuration.Attributes;

    /// <summary>
    /// The step in a single step form.
    /// </summary>
    [SitecoreType(TemplateId = "{62607958-C3F4-4925-BD8E-786E37F10E9A}")]
    public class SingleStep : StepBase
    {
        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        [SitecoreIgnore]
        public override string ViewName
        {
            get
            {
                return "Steps/SingleStep";
            }
        }
    }
}
