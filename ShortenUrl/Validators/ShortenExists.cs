using System.ComponentModel.DataAnnotations;

using Redis.OM;

using ShortenUrl.Models;

namespace ShortenUrl.Validators;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class ShortenExists : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null) return new ValidationResult($"{validationContext.MemberName} is required");
        var url = value.ToString();
        var redisProvider = (RedisConnectionProvider)validationContext.GetService(typeof(RedisConnectionProvider))!;
        var urlsRepo = redisProvider!.RedisCollection<Urls>();
        var existing = urlsRepo.Where(x => x.ShortenUrl == url).FirstOrDefault();
        return existing != null ? new ValidationResult($"{validationContext.MemberName} is exists") : ValidationResult.Success!;
    }
}

