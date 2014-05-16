namespace Unic.Flex.Presentation
{
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Ninject;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.Model.Presentation;

    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString FlexComponent(this HtmlHelper helper, IPresentationComponent component, string htmlFieldPrefix)
        {
            var service = Container.Kernel.Get<IPresentationService>();
            return helper.Partial(
                service.ResolveView(helper.ViewContext, component.ViewName),
                component,
                new ViewDataDictionary(helper.ViewData)
                    {
                        TemplateInfo = new TemplateInfo
                        {
                            HtmlFieldPrefix = htmlFieldPrefix
                        }
                    });
        }
    }
}
