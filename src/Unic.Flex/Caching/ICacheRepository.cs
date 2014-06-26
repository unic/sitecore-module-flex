namespace Unic.Flex.Caching
{
    public interface ICacheRepository
    {
        T Get<T>(string cacheKey) where T : class;

        void AddViewModel(string cacheKey, object data);
    }
}
