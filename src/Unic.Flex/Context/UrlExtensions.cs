using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Context
{
    using Unic.Flex.Model;

    public static class UrlExtensions
    {
        public static string GetUrl(this ItemBase item)
        {
            var context = FlexContext.Current;

            if (context.Item == null) return item.Url;
            if (context.Form == null) return item.Url;

            return string.Join("/", context.Item.Url, item.Url.Split('/').Last());
        }
    }
}
