using ApiContracts.DTO.User;

namespace BlazorApp.Services;

public interface IUserService
{
    public Task<UserDto?> GetUserByIdAsync(int id);
    public Task<List<UserDto>> GetAllUsersAsync(string? username=null);
    public Task<UserDto> AddUserAsync(UserCreateDto request);
    public Task UpdateUserAsync(int id, UserUpdateDto request);
    public Task DeleteUserAsync(int id);

}