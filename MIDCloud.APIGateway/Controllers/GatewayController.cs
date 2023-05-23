using Microsoft.AspNetCore.Mvc;
using MIDCloud.Shared.Models.Interfaces.RabbitMq;

namespace MIDCloud.APIGateway.Controllers;

[Route("api/gateway")]
[ApiController]
public class GatewayController : ControllerBase
{
    private readonly IMessageTransit _rabbitMqService;

    public GatewayController(IMessageTransit rabbitMqService)
    {
        _rabbitMqService = rabbitMqService;
    }
}