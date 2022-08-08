using System.ComponentModel.DataAnnotations;

using Redis.OM.Modeling;

using ShortenUrl.Validators;

namespace ShortenUrl.Models;

[Document(StorageType = StorageType.Json)]
public class Urls
{
    [Indexed]
    [Display(Name = "Shorten URL")]
    [StringLength(60, MinimumLength = 5)]
    [ShortenExists]
    public string ShortenUrl { get; set; }
    [Required]
    [Display(Name = "Original URL")]
    [ValidUrl("{0} is invalid")]
    public string OriginalUrl { get; set; }
    public string? CreatedBy { get; set; }
}