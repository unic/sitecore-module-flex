namespace Unic.Flex.Context
{
    public interface IUrlService
    {
        string AddQueryStringToCurrentUrl(string key, string value);

        string GetCurrentUrl();
    }
}
