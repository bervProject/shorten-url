using System.Net.Http.Headers;

using Microsoft.Net.Http.Headers;

namespace ShortenUrl.Test;

// source code modified from: https://www.stefanhendriks.com/2016/05/11/integration-testing-your-asp-net-core-app-dealing-with-anti-request-forgery-csrf-formdata-and-cookies/

public class CookiesHelper
{

    // Inspired from:
    // https://github.com/aspnet/Mvc/blob/538cd9c19121f8d3171cbfddd5d842cbb756df3e/test/Microsoft.AspNet.Mvc.FunctionalTests/TempDataTest.cs#L201-L202

    public static IDictionary<string, string> ExtractCookiesFromResponse(HttpResponseMessage response)
    {
        IDictionary<string, string> result = new Dictionary<string, string>();
        IEnumerable<string> values;
        if (response.Headers.TryGetValues("Set-Cookie", out values))
        {
            SetCookieHeaderValue.ParseList(values.ToList()).ToList().ForEach(cookie =>
            {
                result.Add(cookie.Name.ToString(), cookie.Value.ToString());
            });
        }
        return result;
    }

    public static FormUrlEncodedContent PutCookiesOnRequest(FormUrlEncodedContent requestMessage, IDictionary<string, string> cookies)
    {
        cookies.Keys.ToList().ForEach(key => requestMessage.Headers.Add("Cookie", new CookieHeaderValue(key, cookies[key]).ToString()));
        return requestMessage;
    }

    public static FormUrlEncodedContent CopyCookiesFromResponse(FormUrlEncodedContent requestMessage, HttpResponseMessage response)
    {
        return PutCookiesOnRequest(requestMessage, ExtractCookiesFromResponse(response));
    }
}
