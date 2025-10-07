using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")] 
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    
    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("{id:int}")]
    public async Task<IResult> GetUserAsync([FromRoute] int id)
    {
        User? user = await _userRepository.GetSingleAsync(id);
        UserDto dto = new()
        {
            Id = user.Id,
            UserName = user.Username
        };
        return Results.Ok(dto);
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> RegisterAsync([FromBody] CreateUserDto request)
    {
        if (VerifyUserNameIsAvailable(request.UserName)) // verify username is available
        {
            User user = new() // create user
            {
                Username = request.UserName,
                Password = request.Password
            };
            User created = await _userRepository.AddAsync(user); // add user to repository
            UserDto dto = new() // create DTO
            {
                Id = created.Id,
                UserName = created.Username
            };
            return Created($"/users/{dto.Id}", dto); // return created userDTO
        }
        else
        {
            return BadRequest($"Username: {request.UserName} already exists.");
        }
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] CreateUserDto request)
    {
        var foundUser = _userRepository.GetMany().Any(u => u.Username == request.UserName && u.Password == request.Password);
        if (foundUser)
        {
            return Ok(new { message = "Login successful!" }); // rewrite it to be usable, for now it's just a placeholder
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
            .Any(u => string.Equals(u.Username, userName, StringComparison.OrdinalIgnoreCase));
    }
}