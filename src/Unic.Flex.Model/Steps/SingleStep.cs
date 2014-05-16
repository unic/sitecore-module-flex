namespace Unic.Flex.Model.Steps
{
    using Glass.Mapper.Sc.Configuration.Attributes;

    [SitecoreType(TemplateId = "{62607958-C3F4-4925-BD8E-786E37F10E9A}")]
    public class SingleStep : StandardStep
    {
        public override string ViewName
        {
            get
            {
                return "Steps/SingleStep";
            }
        }
    }
}
