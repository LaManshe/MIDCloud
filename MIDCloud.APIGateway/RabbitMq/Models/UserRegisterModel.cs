using System.ComponentModel.DataAnnotations;
using MIDCloud.Shared.Models.Interfaces.Users;

namespace MIDCloud.APIGateway.RabbitMq.Models;

public class UserRegisterModel : IRequiredUserFields
{
    [Required]
    public string Login { get; set; } = null!;
    
    [Required]
    public string Password { get; set; } = null!;
}