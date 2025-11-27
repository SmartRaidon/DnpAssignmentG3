using ApiContracts;
using EfcRepositories;
using EfcRepositories.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]

public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDTO>>> GetMany([FromQuery] string? usernameContains = null)
    {
        //base query form repo
        IQueryable<User> query = await _userRepository.GetManyAsync();
        
        //filter from query string
        if (!string.IsNullOrWhiteSpace(usernameContains))
        {
            string lowered = usernameContains.ToLower();
            query = query.Where(u => u.Username.ToLower().Contains(lowered));
        }
        //execute query async in database
        List<User> users = await query.ToListAsync();
        
        //map to DTO
        List<UserDTO> userDtos = MapUsersToDto(users);
        
        return Ok(userDtos);
    }

    // GET by Id action
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetSingle([FromRoute] int id)
    {
        try
        {
            User user = await _userRepository.GetSingleAsync(id);

            UserDTO userDto = MapUserToDto(user);
            return Ok(userDto);
        }
        catch (InvalidOperationException)
        {
            return NotFound($"User with id {id} not found");
        }
        
        
    }
    
    [HttpPost]
    public async Task<ActionResult<UserDTO>> AddUser([FromBody] CreateUserDTO request)
    {
        User userToAdd = new User()
        {
            Password = request.Password,
            Username = request.Username
        };
        User userAdded = await _userRepository.AddAsync(userToAdd);

        UserDTO userToReturn = MapUserToDto(userAdded);
        
        return Ok(userToReturn);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserDTO request)
    {
        User userToUpdate = MapDtoToUser(id, request);
        
        await _userRepository.UpdateAsync(userToUpdate);

        return Ok();
    }
    
    // DELETE action
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await _userRepository.DeleteAsync(id);

        return Ok();
    }

    private UserDTO MapUserToDto(User user)
    {
        return new UserDTO()
        {
            Id = user.Id,
            Username = user.Username,
        };
    }
    
    private User MapDtoToUser(int id, UpdateUserDTO user)
    {
        return new User()
        {
            Id = id,
            Username = user.Username,
            Password = user.Password
        };
    }


    private List<UserDTO> MapUsersToDto(IEnumerable<User> users)
    {
        List<UserDTO> userDtos = new List<UserDTO>();
        foreach (var user in users)
        {
            UserDTO userDto = MapUserToDto(user);
            userDtos.Add(userDto);
        }

        return userDtos;
    }
}