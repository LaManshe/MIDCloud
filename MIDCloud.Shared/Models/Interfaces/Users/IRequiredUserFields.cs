using System.ComponentModel.DataAnnotations;

namespace MIDCloud.Shared.Models.Interfaces.Users;

public interface IRequiredUserFields
{
    [Required]
    string Login { get; set; }
    [Required]
    string Password { get; set; }
}