namespace MIDCloud.APIGateway.Extensions;

public static class HttpContextExtensions
{
    public static void SetToCookie(
        this HttpResponse response, 
        string value, 
        string name,
        DateTimeOffset duration)
    {
        response.Cookies.Append(name, value, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Expires = duration
        });
    }
}