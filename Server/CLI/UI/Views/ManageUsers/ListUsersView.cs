using Entities;
using RepositoryContracts;

namespace CLI.UI.Views.ManageUsers;

public class ListUsersView
{
    private IUserRepository _userRepository;

    public ListUsersView(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task GetManyAsync()
    {
        IQueryable<User> users = await _userRepository.GetManyAsync();

        if (!users.Any())
        {
            Console.WriteLine("No users found");
        }
        else
        {
            Console.WriteLine("Users:");
            foreach (var user in users)
            {
                Console.WriteLine(user.ToString());
            }
        }
        
        await Task.CompletedTask;
    }
}