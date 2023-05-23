using Microsoft.AspNetCore.Mvc;
using MIDCloud.APIGateway.ApiGateways.Interfaces;
using MIDCloud.APIGateway.Extensions;
using MIDCloud.APIGateway.Models;
using MIDCloud.Shared.Models.Interfaces.RabbitMq;
using Newtonsoft.Json;

namespace MIDCloud.APIGateway.Controllers;

[Route("user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IApiGateway _apiGateway;

    public UserController( 
        IHttpClientFactory httpClientFactory, 
        IApiGateway apiGateway)
    {
        _httpClientFactory = httpClientFactory;
        _apiGateway = apiGateway;
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetUser(string id)
    {
        var client = _httpClientFactory.CreateClient("UserRepository");
        
        var userResponse = await client.GetAsync($"user-manager/get?id={id}");

        if (userResponse.IsSuccessStatusCode is false)
        {
            return BadRequest();
        }
        
        var userJson = await userResponse.Content.ReadAsStringAsync();

        return Ok(userJson);
    }

    [HttpGet("authenticate")]
    public async Task<IActionResult> Authenticate([FromForm] UserAuthenticateModel model)
    {
        var userAuthenticateResponse = await _apiGateway.UserManager
            .PostAsync(
                $"user-manager/authenticate?login={model.Login}&password={model.Password}", 
                null);
        
        var tokensJson = await userAuthenticateResponse.Content.ReadAsStringAsync();

        var tokens = JsonConvert.DeserializeObject<Tuple<string, string>>(tokensJson);
        
        Response.SetToCookie(
            tokens!.Item2, 
            "refreshToken", 
            DateTimeOffset.UtcNow.AddDays(30));
        
        return Ok(tokens.Item1);
    }
}