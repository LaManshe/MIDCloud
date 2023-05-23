using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MIDCloud.APIGateway.ApiGateways.Interfaces;
using MIDCloud.Shared.Models.Interfaces.Users;

namespace MIDCloud.APIGateway.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly IApiGateway _apiGateway;

    public JwtMiddleware(RequestDelegate next, IConfiguration configuration, IApiGateway apiGateway)
    {
        _next = next;
        _configuration = configuration;
        _apiGateway = apiGateway;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"]
            .FirstOrDefault()?
            .Split(" ")
            .Last();

        if (token is not null)
        {
            AttachUserToContext(context, token);
        }

        await _next(context);
    }
    
    public void AttachUserToContext(HttpContext context, string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_configuration["Secret"]!);

        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        }, out var validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;

        var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

        context.Items[nameof(IUser)] =_apiGateway.UserManager
            .GetAsync($"users-manager/get?id={userId}");
    }
}