using Microsoft.AspNetCore.Http;
using System;

namespace Task6.WebApp.Helpers;

public static class CookieHelper
{
    public static void SetCookie(HttpResponse response, string key, string value)
    {
        if (response == null)
            throw new ArgumentNullException(nameof(response));

        var options = new CookieOptions
        {
            HttpOnly = false,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Path = "/"
        };

        response.Cookies.Append(key, value, options);
    }

    public static string? GetCookie(HttpRequest request, string key)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        request.Cookies.TryGetValue(key, out var value);
        return value;
    }
}

