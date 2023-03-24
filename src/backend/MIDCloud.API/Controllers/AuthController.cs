using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using MIDCloud.API.Extensions;
using MIDCloud.API.Helpers;
using MIDCloud.API.Models;
using MIDCloud.API.Models.ResponseModels;
using MIDCloud.GlobalInterfaces.Services;

namespace MIDCloud.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ICloudManager _cloudManager;

        public AuthController(ICloudManager cloudManager)
        {
            _cloudManager = Guard.Against.Null(cloudManager, nameof(cloudManager));
        }

        [HttpPost("register")]
        public IActionResult Register([FromForm] UserAuthorizationModel registrationModel)
        {
            if (registrationModel.IsEveryNotEmpty() is false)
            {
                return BadRequest(new ResponseErrorModel("All fields required"));
            }

            var authResult = _cloudManager.RegisterUser(registrationModel);

            if (authResult.IsSuccess is false)
            {
                return BadRequest(new ResponseErrorModel(authResult.ConcatErrors()));
            }

            return Ok(new ResponseSuccessModel($"User {registrationModel.Login} are succsessfully registered"));
        }

        [HttpPost("auth")]
        public IActionResult Auth([FromForm] UserAuthorizationModel authorizationModel)
        {
            var resultAuthentication = _cloudManager.AuthenticateUser(authorizationModel);

            if (resultAuthentication.IsSuccess is false)
            {
                return BadRequest(new ResponseErrorModel(resultAuthentication.Errors.FirstOrDefault()));
            }

            Response.SetRefreshTokenToCookie(resultAuthentication.Value.RefreshToken);

            return Ok(resultAuthentication.Value);
        }

        [HttpGet("refresh")]
        public IActionResult Refresh()
        {
            if (Request.Cookies.ContainsKey("refreshToken") is false)
            {
                return BadRequest(new ResponseErrorModel($"You haven't refresh token in cookie request"));
            }

            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new ResponseErrorModel($"You haven't refresh token in cookie request"));
            }

            var refreshResult = _cloudManager.RefreshAuthenticate(refreshToken);

            if (refreshResult.IsSuccess is false)
            {
                return BadRequest(new ResponseErrorModel(refreshResult.ConcatErrors()));
            }

            Response.SetRefreshTokenToCookie(refreshResult.Value.RefreshToken);

            return Ok(refreshResult.Value);
        }

        [Authorize]
        [HttpGet("check")]
        public IActionResult Check()
        {
            return Ok();
        }
    }
}
