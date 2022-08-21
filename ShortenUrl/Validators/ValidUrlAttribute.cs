namespace ShortenUrl.Validators;

using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class ValidUrlAttribute : ValidationAttribute
{
    public ValidUrlAttribute(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
    public override bool IsValid(object value)
    {
        if (value == null) return false;
        var url = value.ToString();
        var isValid = Uri.TryCreate(url, UriKind.Absolute, out var resultUrl);
        return isValid && resultUrl != null && (resultUrl.Scheme == Uri.UriSchemeHttp || resultUrl.Scheme == Uri.UriSchemeHttps);
    }
}