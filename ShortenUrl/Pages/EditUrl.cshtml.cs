using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Redis.OM;
using Redis.OM.Searching;

using ShortenUrl.Models;
using ShortenUrl.Utils;

namespace ShortenUrl.Pages;

[Authorize]
public class EditUrlModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IRedisCollection<Urls> _urlsRepo;

    public EditUrlModel(ILogger<IndexModel> logger, RedisConnectionProvider provider)
    {
        _logger = logger;
        _urlsRepo = provider.RedisCollection<Urls>();
    }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        PostUrl = await _urlsRepo.FindByIdAsync(id);
        if (PostUrl == null)
        {
            return NotFound();
        }
        return Page();
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
        if (userEmail == null)
        {
            return NotFound();
        }
        var existing = await _urlsRepo.FindByIdAsync(PostUrl.Id.ToString());
        if (existing == null)
        {
            return NotFound();
        }
        if (existing.CreatedBy != userEmail)
        {
            return NotFound();
        }
        existing.OriginalUrl = PostUrl.OriginalUrl;
        existing.ShortenUrl = PostUrl.ShortenUrl;
        await _urlsRepo.UpdateAsync(existing);
        await _urlsRepo.SaveAsync();
        return RedirectToPage("MyUrl");
    }
}