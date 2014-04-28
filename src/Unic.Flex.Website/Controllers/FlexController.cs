using SitecoreContext = Glass.Mapper.Sc.SitecoreContext;

namespace Unic.Flex.Website.Controllers
{
    using System.Web.Mvc;
    using Sitecore.Diagnostics;
    using Sitecore.Mvc.Presentation;
    using Unic.Flex.Context;
    using Unic.Flex.DomainModel;
    using Unic.Flex.DomainModel.Forms;
    using Unic.Flex.Website.Models.Flex;

    public class FlexController : Controller
    {
        private readonly IContextService contextService;
        
        public FlexController(IContextService contextService)
        {
            this.contextService = contextService;
        }
        
        public ActionResult Form()
        {
            var form = FlexContext.Current.Form;
            if (form == null) return new EmptyResult();
            
            return this.View(new FormViewModel { Title = "GET Action for datasource item id: " + form.ItemId });
        }

        [HttpPost]
        public ActionResult Form(FormViewModel model)
        {
            model.Title = "POST Action";
            return this.View(model);
        }
    }
}