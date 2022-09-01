namespace ShortenUrl.Pages;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Redis.OM;
using Redis.OM.Searching;

using ShortenUrl.Models;
using ShortenUrl.Utils;

[Authorize]
public class MyUrlModel : PageModel
{
    private readonly IRedisCollection<Urls> _redisCollection;
    public List<Urls> URLs { get; private set; } = new List<Urls>();
    public MyUrlModel(RedisConnectionProvider provider)
    {
        _redisCollection = provider.RedisCollection<Urls>();
    }
    public void OnGet()
    {
        var userEmail = ClaimUtils.GetEmail(User.Claims);
        URLs = _redisCollection.Where(x => x.CreatedBy == userEmail).OrderByDescending(x => x.VisitedCounter).ToList();
    }

    public async Task<IActionResult> OnPostDeleteAsync(string id)
    {
        var url = await _redisCollection.FindByIdAsync(id);
        if (url == null)
        {
            return NotFound();
        }
        await _redisCollection.DeleteAsync(url);
        await _redisCollection.SaveAsync();
        return RedirectToPage("MyUrl");
    }
}