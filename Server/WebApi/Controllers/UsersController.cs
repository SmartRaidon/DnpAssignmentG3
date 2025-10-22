using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

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
    public async Task<ActionResult<List<UserDTO>>> GetMany()
    {
        IQueryable<User> users = await _userRepository.GetManyAsync();

        List<UserDTO> userDtos = MapUsersToDto(users);
        
        return Ok(userDtos);
    }

    // GET by Id action
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetSingle([FromRoute] int id)
    {
        User user = await _userRepository.GetSingleAsync(id);

        UserDTO userDto = MapUserToDto(user);
        
        return Ok(userDto);
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

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserDTO request)
    {
        User userToUpdate = MapDtoToUser(request);
        
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
    
    private User MapDtoToUser(UpdateUserDTO user)
    {
        return new User()
        {
            Id = user.Id,
            Username = user.Username,
            Password = user.Password
        };
    }


    private List<UserDTO> MapUsersToDto(IQueryable<User> users)
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