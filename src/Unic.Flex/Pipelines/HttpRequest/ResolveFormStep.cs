using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Pipelines.HttpRequest
{
    using Sitecore.Data.Items;
    using Sitecore.Pipelines.HttpRequest;

    public class ResolveFormStep : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            if (Sitecore.Context.Item != null) return;

            // get the current path
            var path = args.LocalPath;
            if (path.EndsWith("/"))
            {
                path = path.Remove(path.Length - 1);
            }

            // resolve the parent item of the current path
            var parentPath = path.Remove(path.LastIndexOf('/'));
            var parentItem = this.ResolveParentItem(parentPath);
            if (parentItem == null) return;
            
            // todo: implement the complete logic
            // 1. check if current item or parent item has a form included
            // 2. check if the current url match to a step in the form config (/ means the first step)
            // 3. if yes -> change context item and load form
            // 4. if no -> show 404
            Sitecore.Context.Item = parentItem;
        }

        private Item ResolveParentItem(string url)
        {
            // todo: test this with display names

            var itemPath = Sitecore.Context.Site.StartPath + Sitecore.MainUtil.DecodeName(url);
            return string.IsNullOrWhiteSpace(itemPath) ? null : Sitecore.Context.Database.GetItem(itemPath);
        }
    }
}
