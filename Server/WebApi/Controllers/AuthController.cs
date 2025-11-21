using ApiContracts.DTO.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]
public class AuthController :ControllerBase
{
    private readonly IUserRepository userRepository;
    
    public AuthController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> LoginAsync([FromBody] LoginRequest request)
    {
        
        if (request == null)
            return BadRequest("Login request is required");

        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return Unauthorized("Username and password are required");

        var loweredUsername = request.Username.ToLower();

        var user = await userRepository.GetMany()
            .Where(u => u.Username.ToLower() == loweredUsername)
            .FirstOrDefaultAsync();

        if (user is null || user.Password != request.Password)
        {
            return Unauthorized("Invalid username or password");
        }

        var dto = new UserDto
        {
            Id = user.Id,
            Username = user.Username
        };

        return Ok(dto);
    }
    
}