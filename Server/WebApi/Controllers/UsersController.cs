using ApiContracts.DTO.User;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserRepository repository;


    public UsersController(IUserRepository repo)
    {
        repository = repo;
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] string? username)
    {
        var userList = repository.GetMany().ToList();
  
        if (!string.IsNullOrWhiteSpace(username))
        {
            userList = userList
                .Where(u => u.Username.Contains(username, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        if (userList.Count == 0) return NotFound("No users found matching criteria.");
        var usersDTO = userList.Select(
            user => new UserDto
            {
                Id = user.Id,
                Username = user.Username
        
            }
        );
        
        return Ok(usersDTO);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var userToFind = await repository.GetSingleAsync(id);
        if (userToFind == null)
        {
            return NotFound();
        }

        var userToDto = new UserDto
        {
            Id = userToFind.Id,
            Username = userToFind.Username
        };
        
        return Ok(userToDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserCreateDto user)
    {
        if (string.IsNullOrWhiteSpace(user.Username))
        {
            return BadRequest("Username is required");
        }

        var userToUser = new User(user.Username, user.Password);

    await repository.AddAsync(userToUser);
        var resultOfUser = new UserDto
        {
            Id = userToUser.Id,
            Username = userToUser.Username
        };
        
        return CreatedAtAction(nameof(GetById), new { id = userToUser.Id }, resultOfUser);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, UserUpdateDto user)
    {
        if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
        {
            return BadRequest("Username and password are required.");
        }
        var userToFind = await repository.GetSingleAsync(id);
        if (userToFind == null || userToFind.Id != id) return NotFound();
        userToFind.Username = user.Username;
        if (!string.IsNullOrWhiteSpace(user.Password))
        {
            userToFind.Password = user.Password;
        }
        await repository.UpdateAsync(userToFind);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var userToFind = await repository.GetSingleAsync(id);
        if (userToFind is null || userToFind.Id != id) return NotFound();
        await repository.DeleteAsync(id);
        return NoContent();
    }
}