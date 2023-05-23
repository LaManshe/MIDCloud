using MIDCloud.Shared.Models.Interfaces.Users;

namespace MIDCloud.UserRepository.Models;

public class User : IUser
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}