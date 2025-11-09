using ApiContracts;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace BlazorApp.Services;

public interface IUserService
{
    public Task<UserDTO> AddUser(CreateUserDTO request);
    public Task UpdateUser(UpdateUserDTO request);
    public Task DeleteUser(int id);
    public Task<UserDTO> GetSingle(int id);
    public Task<List<UserDTO>> GetMany();
    
}