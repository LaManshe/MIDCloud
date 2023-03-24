using Ardalis.GuardClauses;
using DAL.Interfaces;

namespace MIDCloud.API.Models.UserModels
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public AuthResponse(string token, string refreshToken)
        {
            Token = Guard.Against.NullOrEmpty(token, nameof(token));
            RefreshToken = Guard.Against.NullOrEmpty(refreshToken, nameof(refreshToken));
        }
    }
}
