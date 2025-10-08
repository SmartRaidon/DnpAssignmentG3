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

    // GET - /User/{id}
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
    
    // GET - /User
    [HttpGet]
    public async Task<IResult> GetUsers()
    {
        var users = await Task.Run(() => _userRepository.GetMany());
        return Results.Ok(users);
    }

    // POST - create /User
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

    // POST - login /User
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
    
    // PUT - update /User/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<User>> UpdateUser(int id, [FromBody] CreateUserDto request)
    {
        var user = _userRepository.GetMany().FirstOrDefault(u => u.Id == id);
        if (user == null)
            return NotFound("User not found");
        user.Username = request.UserName;
        user.Password = request.Password;
        await _userRepository.UpdateAsync(user);
        return Ok(new { message = "User updated!"});
    }
    
    // DELETE - /User/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<User>> DeleteUser(int id)
    {
        var user = _userRepository.GetMany().FirstOrDefault(u => u.Id == id);
        if (user == null)
            return NotFound("User not found");
        await _userRepository.DeleteAsync(id);
        return Ok(new { message = "User deleted!"});
    }
    
    private bool VerifyUserNameIsAvailable(string userName)
    {
        // Check if a user with the same username already exists
        return !_userRepository
            .GetMany()
            .Any(u => string.Equals(u.Username, userName, StringComparison.OrdinalIgnoreCase));
    }
}