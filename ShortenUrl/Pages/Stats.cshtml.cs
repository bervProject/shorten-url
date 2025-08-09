using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Redis.OM;
using Redis.OM.Searching;

using ShortenUrl.Models;
using ShortenUrl.Utils;

namespace ShortenUrl.Pages;

public class StatsModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IRedisCollection<Urls> _urlsRepo;

    public StatsModel(ILogger<IndexModel> logger, RedisConnectionProvider provider)
    {
        _logger = logger;
        _urlsRepo = provider.RedisCollection<Urls>();
    }

    public int TotalCreatedLink { get; set; } = default!;
    public List<Urls> MostVisitedUrls { get; set; } = new List<Urls>();

    public async Task OnGetAsync()
    {
        TotalCreatedLink = await _urlsRepo.CountAsync();
        MostVisitedUrls.AddRange(_urlsRepo.Where(x => x.VisitedCounter > 0).OrderByDescending(x => x.VisitedCounter).Take(10));
    }
}