using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Helpers
{
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Ninject;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.DomainModel.Presentation;
    using Unic.Flex.Presentation;

    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString FlexComponent(this HtmlHelper helper, IPresentationComponent component)
        {
            var service = Container.Kernel.Get<IPresentationService>();
            return helper.Partial(service.ResolveView(helper.ViewContext, component.ViewName), component);
        }
    }
}
