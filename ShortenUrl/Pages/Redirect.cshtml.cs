using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Redis.OM;
using Redis.OM.Searching;

using ShortenUrl.Models;

namespace ShortenUrl.Pages
{
    public class RedirectModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IRedisCollection<Urls> _urlsRepo;

        public RedirectModel(ILogger<IndexModel> logger, RedisConnectionProvider provider)
        {
            _logger = logger;
            _urlsRepo = provider.RedisCollection<Urls>();
        }
        public IActionResult OnGet(string url)
        {
            var existing = _urlsRepo.Where(x => x.ShortenUrl == url).FirstOrDefault();
            if (existing == null)
            {
                return Redirect("/");
            }
            existing.VisitedCounter += 1;
            _urlsRepo.Update(existing);
            _urlsRepo.Save();
            return Redirect(existing.OriginalUrl);
        }
    }
}