using Microsoft.EntityFrameworkCore;
using newsapi.DTOs;
using newsapi.EFs;
using newsapi.Interfaces;

namespace newsapi.Services;

public class NewsService : INewsService
{
    private readonly TestDbContext _testDbContext;
    private readonly ILogger<NewsService> _logger;
    private readonly IInMemoryCacheService _cache;

    public NewsService(TestDbContext testDbContext, ILogger<NewsService> logger, IInMemoryCacheService cache)
    {
        _testDbContext = testDbContext;
        _logger = logger;
        _cache = cache;
    }

    public async Task<List<News>> GetNewsAsyncNoLock()
    {
        return await GetNewsFromCacheNoLock();
    }
    public async Task<List<News>> GetNewsAsyncWithLock()
    {
        
        return await GetNewsFromCacheWithLock();
    }

    private static readonly SemaphoreSlim _locker = new SemaphoreSlim(initialCount:1, maxCount:1);

    #region GetNewsFromCacheWithLock, new method, use SemaphoreSlim to limit only one thread can enter Critical Section only
    /// <summary>
    /// new method, use SemaphoreSlim to limit only one thread can enter Critical Section only
    /// </summary>
    /// <returns></returns>
    private async Task<List<News>> GetNewsFromCacheWithLock()
    {
        const string NEWS1_CACHE = "__NEWS1_CACHE__";

        if ( _cache.KeyExist(NEWS1_CACHE) )
        {
            _logger.LogInformation("Get news from cache");
            return _cache.GetFromCache<List<News>>(NEWS1_CACHE)!;
        }
        else 
        {
            _logger.LogInformation("Wait _locker");
            await _locker.WaitAsync();

            _logger.LogInformation("Got _locker");
            if ( _cache.KeyExist(NEWS1_CACHE) )
            {
                _logger.LogInformation("Get news from cache and release lock (after got locker)");

                _locker.Release();   
                return _cache.GetFromCache<List<News>>(NEWS1_CACHE)!;
            }
            else 
            {
                _logger.LogInformation("Prepare cache data, read from db");
                try
                {
                    var dbNews = await _testDbContext.News1.FromSqlInterpolated<News1>($"usp_GetNews1 2000").AsNoTracking().ToListAsync();

                    var dtos = dbNews.Select(entity => new News
                    {
                        NewsId = entity.NewsId,
                        Content = entity.Content,
                        CreateTimeStamp = entity.CreateTimeStamp
                    }).ToList();

                    _logger.LogInformation("set news to cache");
                    _cache.SetCache(NEWS1_CACHE, dtos, TimeSpan.FromMinutes(1), null);
                    return dtos;
                }
                catch (System.Exception e)
                {
                    _logger.LogError(e, "xxx");   
                    throw;
                }
                finally
                {
                    _logger.LogInformation("Prepare cache data, release lock");
                    _locker.Release();
                }
            }
        }
    }
    #endregion

    #region GetNewsFromCacheNoLock, old method, no lock
    /// <summary>
    /// old method, no lock
    /// </summary>
    /// <returns></returns>
    private async Task<List<News>> GetNewsFromCacheNoLock()
    {
        const string NEWS1_CACHE = "__NEWS1_CACHE_NO_LOCK__";

        if ( _cache.KeyExist(NEWS1_CACHE) )
        {
            _logger.LogInformation("Get news from cache");
            return _cache.GetFromCache<List<News>>(NEWS1_CACHE)!;
        }
        else 
        {
            _logger.LogInformation("Get news from db, prepare cache data, read from db");
            try
            {
                var dbNews = await _testDbContext.News1.FromSqlInterpolated<News1>($"usp_GetNews1 2000").AsNoTracking().ToListAsync();

                var dtos = dbNews.Select(entity => new News
                {
                    NewsId = entity.NewsId,
                    Content = entity.Content,
                    CreateTimeStamp = entity.CreateTimeStamp
                }).ToList();

                _logger.LogInformation("set news to cache");
                _cache.SetCache(NEWS1_CACHE, dtos, TimeSpan.FromMinutes(1), null);
                return dtos;
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "xxx");   
                throw;
            }
        }
    }
    #endregion 
}
