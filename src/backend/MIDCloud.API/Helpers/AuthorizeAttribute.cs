using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using MIDCloud.GlobalInterfaces.Users;

namespace MIDCloud.API.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (IUser)context.HttpContext.Items[nameof(IUser)];

            if (user is null)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
