using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using newsapi.Interfaces;

namespace newsapi.Services;

public class InMemoryCacheService : IInMemoryCacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<InMemoryCacheService> _logger;
    
    /// <summary>
    /// c'tor
    /// </summary>
    /// <param name="memoryCache"></param>
    /// <param name="logger"></param>
    public InMemoryCacheService(IMemoryCache memoryCache, ILogger<InMemoryCacheService> logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public void ClearCache(string key)
    {
        try
        {
            _memoryCache.Remove(key);
        }
        catch (System.Exception err)
        {
            _logger.LogWarning(err, $"ClearCache {key} error");
        }   
    }

    public T? GetFromCache<T>(string key) where T : class
    {
        string vv;

        if ( _memoryCache.TryGetValue(key, out vv) )
        {
            T? result = JsonSerializer.Deserialize<T>(vv);
            return result;
        }
        else
        {
            return null;
        }
    }

    public bool KeyExist(string key)
    {
        object vv;

        return _memoryCache.TryGetValue(key, out vv);
    }

    public void SetCache<T>(string key, T value, TimeSpan? absolutedExpireTime, TimeSpan? unusedExpireTime) where T : class
    {
        MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions();
        cacheEntryOptions.SetAbsoluteExpiration(absolutedExpireTime ?? TimeSpan.FromMinutes(5));
        
        if ( unusedExpireTime != null )
            cacheEntryOptions.SetSlidingExpiration(unusedExpireTime.Value);

        var response = JsonSerializer.Serialize(value);
        _memoryCache.Set<string>(key, response, cacheEntryOptions);
    }
}
