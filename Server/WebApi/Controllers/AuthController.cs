using ApiContracts.DTO.User;
using Microsoft.AspNetCore.Mvc;
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
        {
            return BadRequest("Login request is required");
        }
    
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return Unauthorized("Username and password are required");
        }

        List<Entities.User> matches = userRepository.GetMany()
            .Where(u => u.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (matches.Count == 0)
        {
            return Unauthorized("Invalid username or password");
        }

        Entities.User? user = matches.FirstOrDefault(u => u.Password == request.Password);
        if (user is null)
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