namespace Unic.Flex.Caching
{
    public interface ICacheRepository
    {
        T Get<T>(string cacheKey) where T : class;

        void Add(string cacheKey, object data);
    }
}
