using ApiContracts;
using ApiContracts.Authentication;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("auth")]

public class AuthenticationController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public AuthenticationController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> AddUser([FromBody] LoginRequest request)
    {
        //find user by username
        IQueryable<User> users = await _userRepository.GetManyAsync();
        User? user = users.SingleOrDefault(u => u.Username == request.Username);
         
        //if user doesnt exist
        if (user is null)
        {
            return Unauthorized("User does not exist");
        }

        if (user.Username != request.Username)
        {
            return Unauthorized("Invalid username");
        }
        
        if (user.Password != request.Password)
        {
            return Unauthorized("Invalid password");
        }
        //map user to userDTO
        var userDTO = new UserDTO
        {
            Id = user.Id,
            Username = user.Username
        };
        return userDTO;
    }
}