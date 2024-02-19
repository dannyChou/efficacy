using newsapi.DTOs;

namespace newsapi.Interfaces;

public interface INewsService
{
    Task<List<News>> GetNewsAsyncNoLock();
    Task<List<News>> GetNewsAsyncWithLock();
}
