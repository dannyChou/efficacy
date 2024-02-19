using Microsoft.AspNetCore.Mvc;
using newsapi.DTOs;
using newsapi.EFs;
using newsapi.Interfaces;

namespace newsapi.Controllers;

[Route("[controller]")]
public class NewsController: ControllerBase
{
    private ILogger<NewsController> _logger;
    private INewsService _newsService;

    public NewsController(ILogger<NewsController> logger, INewsService newsService)
    {
        _logger = logger;
        _newsService = newsService;
    }

    [HttpGet("news1")]
    public async Task<List<News>> GetNewsNoLock()
    {
        using ( _logger.BeginScope(new[] { new KeyValuePair<string, object>("P1", Guid.NewGuid()) } ))
        {
            return await _newsService.GetNewsAsyncNoLock();
        } 
    }

    [HttpGet("news2")]
    public async Task<List<News>> GetNewsWithLock()
    {
        using ( _logger.BeginScope(new[] { new KeyValuePair<string, object>("P1", Guid.NewGuid()) } ))
        {
            return await _newsService.GetNewsAsyncWithLock();
        } 
    }

    [HttpGet("servererror")]
    public void ServerError() 
    {
        int a=0, b=1, c;
        
        c=b/a;

    }
}
