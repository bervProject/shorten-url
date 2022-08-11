using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Net.Http.Headers;

namespace ShortenUrl.Test;

public class TestLogin : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public TestLogin(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    public static async Task<FormUrlEncodedContent> CopyCsrfToken(HttpClient client, List<KeyValuePair<string, string>> requestBody)
    {
        var response = await client.GetAsync("/");

        var csrf = GetCsrfFromResponseHeaders(response.Headers);

        requestBody.Add(new KeyValuePair<string, string>("__RequestVerificationToken", csrf));
        var formData = new FormUrlEncodedContent(requestBody);
        var copyCookies = CookiesHelper.CopyCookiesFromResponse(formData, response);
        return copyCookies;
    }

    private static string GetCsrfFromResponseHeaders(HttpResponseHeaders headers)
    {
        if (headers.TryGetValues("Set-Cookie", out var result))
        {
            var cookie = result.FirstOrDefault(x => x.StartsWith(".AspNetCore.Antiforgery"));
            if (cookie == null)
            {
                return string.Empty;
            }
            var cookieParts = cookie.Split(";");
            if (cookieParts.Length != 4)
            {
                return string.Empty;
            }
            var firstPart = cookieParts.FirstOrDefault();
            if (firstPart == null)
            {
                return string.Empty;
            }
            var valueParts = firstPart.Split("=");
            if (valueParts.Length != 2)
            {
                return string.Empty;
            }
            return valueParts[1];
        }
        else
        {
            return string.Empty;
        }
    }

    [Fact]
    public async Task TestGetRoot()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(MediaTypeNames.Text.Html, response.Content.Headers.ContentType?.MediaType);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("Original URL", responseBody);
        Assert.Contains("Shorten URL", responseBody);
        var csrfToken = GetCsrfFromResponseHeaders(response.Headers);
        Assert.NotEqual(string.Empty, csrfToken);
    }

    [Fact]
    public async Task TestGetLogin()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });
        var response = await client.GetAsync("/Login");
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Equal(MediaTypeNames.Text.Html, response.Content.Headers.ContentType?.MediaType);
        var headerLocationQuery = response.Headers.Location?.Query;
        var path = response.Headers.Location?.AbsolutePath;
        Assert.Contains("/authorize", path);
        Assert.Contains("redirect_uri", headerLocationQuery);
        Assert.Contains("response_type=id_token", headerLocationQuery);
    }


    [Fact]
    public async Task TestPostRootWithoutData()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });
        var data = new List<KeyValuePair<string, string>>();
        var requestData = await CopyCsrfToken(client, data);
        var response = await client.PostAsync("/", requestData);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(response.Content.Headers.ContentType?.MediaType);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Equal("", responseBody);
    }

    [Fact]
    public async Task TestPostRootWithInvalidData()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });
        var data = new List<KeyValuePair<string, string>>();
        var random = new Random();
        var shortenUrl = $"test-{random.NextInt64()}";
        var originalUrl = "https://berviantoleo.my.id";
        data.Add(new KeyValuePair<string, string>("PostUrl.OriginalUrl", originalUrl));
        data.Add(new KeyValuePair<string, string>("PostUrl.ShortenUrl", shortenUrl));
        var formData = await CopyCsrfToken(client, data);
        var response = await client.PostAsync("/", formData);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(response.Content.Headers.ContentType?.MediaType);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Equal("", responseBody);
    }
}