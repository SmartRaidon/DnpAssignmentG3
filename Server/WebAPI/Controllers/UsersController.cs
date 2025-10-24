using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")] 
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    
    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // GET - /Users/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserAsync([FromRoute] int id)
    {
        User user = await _userRepository.GetSingleAsync(id);
        UserDto dto = new()
        {
            Id = user.Id,
            UserName = user.Username
        };
        return Ok(dto);
    }
    
    // GET - /Users
    [HttpGet]
    public async Task<IActionResult> GetUsersAsync()
    {
        var users = await Task.Run(() => _userRepository.GetMany()
            .Select(u => new UserDto
        {
            Id = u.Id,
            UserName = u.Username
        }).ToList());
        return Ok(users);
    }

    // POST - create /Users
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

    // POST - login /Users
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
    
    // PUT - update /Users/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> UpdateUserAsync([FromRoute] int id, [FromBody] CreateUserDto request)
    {
        var user = _userRepository.GetMany().FirstOrDefault(u => u.Id == id);
        if (user == null)
            return NotFound("User not found");
        user.Username = request.UserName;
        user.Password = request.Password;
        await _userRepository.UpdateAsync(user);
        var response = new UserDto
        {
            Id = user.Id,
            UserName = user.Username
        };
        return Ok(new
        {
            message = "User successfully updated!",
            user = response
        });
    }
    
    // DELETE - /Users/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<User>> DeleteUserAsync([FromRoute] int id)
    {
        var user = _userRepository.GetMany().FirstOrDefault(u => u.Id == id);
        if (user == null)
            return NotFound("User not found");
        await _userRepository.DeleteAsync(id);
        return NoContent();
    }
    
    private bool VerifyUserNameIsAvailable(string userName)
    {
        // Check if a user with the same username already exists
        return !_userRepository
            .GetMany()
            .Any(u => string.Equals(u.Username, userName, StringComparison.OrdinalIgnoreCase));
    }
}