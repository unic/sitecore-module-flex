namespace Unic.Flex.Website.Controllers
{
    using System.Web.Mvc;
    using Unic.Flex.Website.Models.Flex;

    public class FlexController : Controller
    {
        public ActionResult Form()
        {
            return this.View(new FormViewModel { Title = "GET Action"});
        }

        [HttpPost]
        public ActionResult Form(FormViewModel model)
        {
            model.Title = "POST Action";
            return this.View(model);
        }
    }
}