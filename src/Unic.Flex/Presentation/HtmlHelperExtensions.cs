namespace Unic.Flex.Presentation
{
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Ninject;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.DomainModel.Presentation;

    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString FlexComponent(this HtmlHelper helper, IPresentationComponent component)
        {
            var service = Container.Kernel.Get<IPresentationService>();
            return helper.Partial(service.ResolveView(helper.ViewContext, component.ViewName), component);
        }
    }
}
