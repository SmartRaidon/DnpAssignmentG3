using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private IUserRepository _userRepository;

    public CreateUserView(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task AddUserAsync(string username, string password)
    {
        if(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentNullException(nameof(username),"Cannot be null or empty");
        }
        if(string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentNullException(nameof(password),"Cannot be null or empty");
        }
        
        User userToAdd = new User
        {
            Username = username,
            Password = password
        };
        User createdUser = await _userRepository.AddAsync(userToAdd);
        Console.WriteLine($"User created with details: {createdUser}");
    }
}