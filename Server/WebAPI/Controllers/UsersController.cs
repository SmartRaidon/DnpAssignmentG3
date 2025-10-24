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

    [HttpPost] 
    public async Task<ActionResult<UserDto>> CreateUserAsync([FromBody] CreateUserDto request) 
    {
        if (VerifyUserNameIsAvailable(request.UserName))
        {
            var user = new User(request.UserName, request.Password);
            User created = await _userRepository.AddAsync(user);
                
                    
            UserDto dto = new()
            {
                Id = created.Id,
                UserName = created.Username 
            }; 
            return Created($"/users/{dto.Id}", created);
        }
        else
        {
            return BadRequest("Invalid username or password");
        }
    }
    
    // GET - /Users/{id}
    [HttpGet("{id:int}")]
    public async 
    }
    
    private bool VerifyUserNameIsAvailable(string userName)
    {
        // Check if a user with the same username already exists
        return !_userRepository
            .GetMany()
            .Any(u => string.Equals(u.Username, userName, StringComparison.OrdinalIgnoreCase));
    }
}