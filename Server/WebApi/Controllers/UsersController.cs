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
    public IActionResult GetAll()
    {
        var userList = repository.GetMany().ToList();
        if (userList.Count == 0 || userList == null) return NotFound();
        var usersDTO = userList.Select(
            user => new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
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
            Username = userToFind.Username,
            Password = userToFind.Password
        };
        
        return Ok(userToDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserCreateDto user)
    {
        var userToUser = new User
        {
            Username = user.Username,
            Password = user.Password
        };
        
        await repository.AddAsync(userToUser);
        var resultOfUser = new UserDto
        {
            Id = userToUser.Id,
            Username = userToUser.Username,
            Password = userToUser.Password
        };
        
        return CreatedAtAction(nameof(GetById), new { id = userToUser.Id }, resultOfUser);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, UserUpdateDto user)
    {
        var userToFind = await repository.GetSingleAsync(id);
        if (userToFind == null || userToFind.Id != id) return NotFound();
        userToFind.Username = user.Username;
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