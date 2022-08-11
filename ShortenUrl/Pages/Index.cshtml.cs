using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Redis.OM;
using Redis.OM.Searching;

using ShortenUrl.Models;
using ShortenUrl.Utils;

namespace ShortenUrl.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IRedisCollection<Urls> _urlsRepo;

    public IndexModel(ILogger<IndexModel> logger, RedisConnectionProvider provider)
    {
        _logger = logger;
        _urlsRepo = provider.RedisCollection<Urls>();
    }

    public void OnGet()
    {
    }

    [BindProperty]
    public Urls? PostUrl { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || PostUrl == null)
        {
            return Page();
        }
        var userEmail = ClaimUtils.GetEmail(User.Claims);
        if (userEmail != null)
        {
            PostUrl.CreatedBy = userEmail;
        }
        _urlsRepo.Insert(PostUrl);
        await _urlsRepo.SaveAsync();
        return Redirect("/");
    }
}
