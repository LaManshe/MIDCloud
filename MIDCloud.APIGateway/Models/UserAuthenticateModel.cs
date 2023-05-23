using MIDCloud.Shared.Models.Interfaces.Users;

namespace MIDCloud.APIGateway.Models;

public class UserAuthenticateModel : IRequiredUserFields
{
    public string Login { get; set; }
    public string Password { get; set; }
}