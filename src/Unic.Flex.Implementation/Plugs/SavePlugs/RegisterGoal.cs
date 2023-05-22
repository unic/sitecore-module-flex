namespace Unic.Flex.Implementation.Plugs.SavePlugs
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Model.Forms;
    using Model.Plugs;


    [SitecoreType(TemplateId = "{9A7F5FA1-E267-4BF2-8D3C-4D487792B636}")]
    public class RegisterGoal : SavePlugBase
    {
        public override bool IsAsync { get; }
        public override void Execute(IForm form)
        {
            throw new System.NotImplementedException();
        }
    }
}
