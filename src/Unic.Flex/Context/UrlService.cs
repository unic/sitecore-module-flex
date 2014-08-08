namespace Unic.Flex.Context
{
    public class UrlService : IUrlService
    {
        public string AddQueryStringToCurrentUrl(string key, string value)
        {
            var rawUrl = this.GetCurrentUrl();
            return string.Format("{0}{1}{2}={3}", rawUrl, rawUrl.Contains("?") ? "&" : "?", key, value);
        }

        public string GetCurrentUrl()
        {
            return Sitecore.Web.WebUtil.GetRawUrl();
        }
    }
}
