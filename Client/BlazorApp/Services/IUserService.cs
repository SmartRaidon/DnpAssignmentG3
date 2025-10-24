using System.Threading.Tasks;
using System.Collections.Generic;
using ApiContracts;

namespace BlazorApp.Http.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> AddUserAsync(CreateUserDto request);
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        //Task UpdateUserAsync(int id, UpdateUserDto request);
        Task DeleteUserAsync(int id);
    }
}