namespace ShortenUrl.Utils;
public static class ClaimUtils
{
    public static string? GetEmail(IEnumerable<System.Security.Claims.Claim> claims)
    {
        var emailClaims = claims.Where(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase) || x.Type.Contains("email", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        return emailClaims?.Value;
    }
}

