using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")] 
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    
    public AuthController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    // POST - create /Users
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> RegisterAsync([FromBody] CreateUserDto request)
    {
        if (VerifyUserNameIsAvailable(request.Username)) // verify username is available
        {
            User user = new() // create user
            {
                Username = request.Username,
                Password = request.Password
            };
            User created = await _userRepository.AddAsync(user); // add user to repository
            UserDto dto = new() // create DTO
            {
                Id = created.Id,
                Username = created.Username
            };
            return Created($"/users/{dto.Id}", dto); // return created userDTO
        }
        else
        {
            return BadRequest($"Username: {request.Username} already exists.");
        }
    }

    // POST - login /Users
    [HttpPost("login")]
    public IActionResult Login([FromBody] CreateUserDto request)
    {
        var foundUser = _userRepository.GetMany().FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);
        if (foundUser != null)
        {
            var userDto = new UserDto
            {
                Id = foundUser.Id,
                Username = foundUser.Username
            };
            return Ok(userDto);
        }
        else
        {
            return Unauthorized("Invalid username or password.");
        }
    }
    
    private bool VerifyUserNameIsAvailable(string userName)
    {
        // Check if a user with the same username already exists
        return !_userRepository
            .GetMany()
            .Any(u => u.Username == userName);
    }
}