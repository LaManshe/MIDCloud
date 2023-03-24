using MIDCloud.GlobalInterfaces.Users;

namespace MIDCloud.API.Models
{
    public class UserAuthorizationModel : IMinimalUser
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public bool IsEveryNotEmpty()
        {
            return
                string.IsNullOrEmpty(Login) is false ||
                string.IsNullOrEmpty(Password) is false;
        }
    }
}
