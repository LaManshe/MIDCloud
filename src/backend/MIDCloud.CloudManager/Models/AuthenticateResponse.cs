using MIDCloud.GlobalInterfaces.Responses;

namespace MIDCloud.CloudManager.Models
{
    internal class AuthenticateResponse : IAuthenticate
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public AuthenticateResponse(string token, string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }

        public AuthenticateResponse((string, string) tokens)
        {
            Token = tokens.Item1;
            RefreshToken = tokens.Item2;
        }
    }
}
