using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class SingleUsersView
{
    private IUserRepository _userRepository;

    public SingleUsersView(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task GetSingleAsync(int id)
    {
        User userToGet = await _userRepository.GetSingleAsync(id);
        Console.WriteLine($"Current user: {userToGet}");
        
        await Task.CompletedTask;
    }
}