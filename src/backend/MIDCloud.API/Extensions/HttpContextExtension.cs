using Ardalis.GuardClauses;
using JetBrains.Annotations;
using MIDCloud.GlobalInterfaces.Users;

namespace MIDCloud.API.Extensions
{
    public static class HttpContextExtension
    {
        public static IUser? GetRequesterUser(this HttpContext context)
        {
            if (context.Items[nameof(IUser)] is IUser registeredUser)
            {
                return registeredUser;
            }

            return null;
        }

        public static void SetRefreshTokenToCookie(this HttpResponse response, string token)
        {
            response.Cookies.Append("refreshToken", token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });
        }

        public static void SetResponseMaxPageHeader(this HttpResponse response, string value)
        {
            Guard.Against.NullOrEmpty(value, nameof(value));

            response.Headers.Add("max-page", value);
        }
    }
}
