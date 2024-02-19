namespace newsapi.Interfaces;

public interface IInMemoryCacheService
{
    T? GetFromCache<T>(string key) where T : class;
        
    void SetCache<T>(string key, T value, TimeSpan? absolutedExpireTime, TimeSpan? unusedExpireTime) where T : class;

    void ClearCache(string key);

    bool KeyExist(string key);
}
