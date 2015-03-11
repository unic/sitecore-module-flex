namespace Unic.Flex.Model.Fields
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.Components;

    /// <summary>
    /// Reusable field model.
    /// </summary>
    [SitecoreType(TemplateId = "{C71F2A06-56B1-4045-A96E-FDF1F3E76284}")]
    public class ReusableField : FieldBase<object>, IInvalidComponent
    {
        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        /// <exception cref="System.NotImplementedException">This field has no view</exception>
        public override string ViewName
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
