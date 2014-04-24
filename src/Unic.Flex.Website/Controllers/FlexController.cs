namespace Unic.Flex.Website.Controllers
{
    using System.Web.Mvc;
    using Glass.Mapper.Sc;
    using Sitecore.Diagnostics;
    using Sitecore.Mvc.Presentation;
    using Unic.Flex.DomainModel;
    using Unic.Flex.Website.Models.Flex;

    public class FlexController : Controller
    {
        public ActionResult Form()
        {
            var dataSource = RenderingContext.Current.Rendering.DataSource;
            Assert.IsTrue(Sitecore.Data.ID.IsID(dataSource), "Datasource is not valid");

            var formItem = (new SitecoreContext()).GetItem<BaseItem>(dataSource);

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