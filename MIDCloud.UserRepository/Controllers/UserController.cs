using Microsoft.AspNetCore.Mvc;
using MIDCloud.Shared.Models.Interfaces.Users;
using MIDCloud.UserRepository.Helpers;
using MIDCloud.UserRepository.Models;
using MIDCloud.UserRepository.Repository.Interfaces;

namespace MIDCloud.UserRepository.Controllers;

[Route("user-manager")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IRepository<IUser> _repository;

    public UserController(IRepository<IUser> repository)
    {
        _repository = repository;
    }

    [HttpGet("get")]
    public IActionResult Get(string id)
    {
        if (int.TryParse(id, out var idNumber) is false)
        {
            return BadRequest();
        }
        
        var user = _repository.Get(idNumber);

        return user is not null 
            ? Ok(user) 
            : BadRequest();
    }

    [HttpPost("authenticate")]
    public IActionResult Authenticate(string login, string password)
    {
        var user = _repository.Items
            .First(user => user.Login == login && user.Password == password);

        var token = Jwt.GenerateJwtToken(new User()
        {
            Login = user.Login,
            Password = user.Password
        });

        var refreshToken = Jwt.GenerateJwtRefreshToken(new User()
        {
            Login = user.Login,
            Password = user.Password
        });
        
        return Ok(new Tuple<string, string>(token, refreshToken));
    }
}