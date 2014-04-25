﻿using SitecoreContext = Glass.Mapper.Sc.SitecoreContext;

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
        private IContextService contextService;
        
        public FlexController(IContextService contextService)
        {
            this.contextService = contextService;
        }
        
        public ActionResult Form()
        {
            var dataSource = RenderingContext.Current.Rendering.DataSource;
            Assert.IsTrue(Sitecore.Data.ID.IsID(dataSource), "Datasource is not valid");

            var formItem = this.contextService.LoadForm(dataSource, new SitecoreContext());

            return this.View(new FormViewModel { Title = "GET Action for datasource item id: " + formItem.ItemId });
        }

        [HttpPost]
        public ActionResult Form(FormViewModel model)
        {
            model.Title = "POST Action";
            return this.View(model);
        }
    }
}