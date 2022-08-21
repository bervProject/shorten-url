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
        var id = ((ShortenUrl.Models.Urls)validationContext.ObjectInstance).Id;
        var url = value.ToString();
        var redisProvider = (RedisConnectionProvider)validationContext.GetService(typeof(RedisConnectionProvider))!;
        var urlsRepo = redisProvider!.RedisCollection<Urls>();
        var existing = urlsRepo.Where(x => x.ShortenUrl == url).FirstOrDefault();


        if (existing != null && id == Ulid.Empty)
        {
            return new ValidationResult($"{validationContext.MemberName} is exists");
        }
        else if (existing != null && existing.Id == id)
        {
            return ValidationResult.Success!;
        }
        else if (existing == null)
        {
            return ValidationResult.Success!;
        }
        else
        {
            return new ValidationResult($"{validationContext.MemberName} is exists");
        }
    }
}