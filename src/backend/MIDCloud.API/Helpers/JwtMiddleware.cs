 using DAL.Interfaces;
using Microsoft.IdentityModel.Tokens;
using MIDCloud.API.Services.Interfaces;
using MIDCloud.GlobalInterfaces.Services;
using MIDCloud.GlobalInterfaces.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MIDCloud.API.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        //private readonly ILogger _logger;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context, ICloudManager cloudManager)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                AttachUserToContext(context, cloudManager, token);

            await _next(context);
        }

        public void AttachUserToContext(HttpContext context, ICloudManager cloudManager, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(_configuration["Secret"]);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                context.Items[nameof(IUser)] = cloudManager.UserService.Get(userId);
            }
            catch
            {
                // todo: need to add logger
            }
        }
    }
}
