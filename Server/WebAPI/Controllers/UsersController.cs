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
            Username = user.Username
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
            Username = u.Username
        }).ToList());
        return Ok(users);
    }
    
    // PUT - update /Users/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> UpdateUserAsync([FromRoute] int id, [FromBody] CreateUserDto request)
    {
        var user = _userRepository.GetMany().FirstOrDefault(u => u.Id == id);
        if (user == null)
            return NotFound("User not found");
        user.Username = request.Username;
        user.Password = request.Password;
        await _userRepository.UpdateAsync(user);
        var response = new UserDto
        {
            Id = user.Id,
            Username = user.Username
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
}