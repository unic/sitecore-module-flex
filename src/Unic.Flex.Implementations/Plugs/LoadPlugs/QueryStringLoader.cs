namespace Unic.Flex.Model.DomainModel.Plugs.LoadPlugs
{
    using System.Linq;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Sitecore.Diagnostics;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Sections;

    [SitecoreType(TemplateId = "{4906DF7C-B200-4825-B1AC-488D7D928452}")]
    public class QueryStringLoader : LoadPlugBase
    {
        // todo: move all plug, fields, etc. implementation to custom assembly -> we have a circulate project references else
        
        public override void Execute(Form form)
        {
            Assert.ArgumentNotNull(form, "form");

            // todo: implement

            ((StandardSection)form.Steps.First().Sections.First()).Fields.First().Value = "blupp";
        }
    }
}
