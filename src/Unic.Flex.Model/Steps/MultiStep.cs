namespace Unic.Flex.Model.Steps
{
    using Glass.Mapper.Sc.Configuration.Attributes;

    [SitecoreType(TemplateId = "{93EC9DA3-81FC-4A1E-82F8-DD988A2D355B}")]
    public class MultiStep : StandardStep
    {
        public override string ViewName
        {
            get
            {
                return "Steps/MultiStep";
            }
        }
    }
}
