﻿namespace Unic.Flex.Website.Controllers
{
    using System.Web.Mvc;
    using Unic.Flex.Context;
    using Unic.Flex.DomainModel.Steps;
    using Unic.Flex.Presentation;
    using Unic.Flex.Website.Models.Flex;

    public class FlexController : Controller
    {
        private readonly IPresentationService presentationService;

        public FlexController(IPresentationService presentationService)
        {
            this.presentationService = presentationService;
        }
        
        public ActionResult Form()
        {
            var form = FlexContext.Current.Form;
            if (form == null) return new EmptyResult();

            var formView = this.presentationService.ResolveView(this.ControllerContext, form.ViewName);
            
            var model = new FormViewModel
                            {
                                Title = form.Title,
                                Introdcution = form.Introduction,
                                Step = form.GetActiveStep()
                            };

            return this.View(formView, model);
        }

        [HttpPost]
        public ActionResult Form(FormViewModel model)
        {
            model.Title = "POST Action";
            return this.View("~/Views/Modules/Flex//Default/Form.cshtml");
        }
    }
}