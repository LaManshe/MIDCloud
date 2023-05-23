using MIDCloud.APIGateway.ApiGateways.Interfaces;

namespace MIDCloud.APIGateway.ApiGateways;

public class ApiGateways : IApiGateway
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpClient UserManager => _httpClientFactory.CreateClient("UserRepository");

    public ApiGateways(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
}