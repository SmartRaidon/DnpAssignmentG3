using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView
{
    private IUserRepository _userRepository;

    public ListUsersView(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task GetManyAsync()
    {
        IQueryable<User> users = _userRepository.GetMany();
        Console.WriteLine("Users:");
        foreach (var user in users)
        {
            Console.WriteLine(user.ToString());
        }
        await Task.CompletedTask;
    }
}