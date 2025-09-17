using RepositoryContracts;
using Entities;

namespace CLI.UI;

public class UserView
{
    private readonly IUserRepository _userRepository;

    public UserView(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task ShowUserManagement()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== User Management ===");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. List Users");
            Console.WriteLine("3. Back to Main Menu");
            Console.Write("Select an option: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await CreateUserAsync();
                    break;
                case "2":
                    await ListUsersAsync();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task CreateUserAsync()
    {
        Console.Write("Enter username: ");
        var username = Console.ReadLine();
        Console.Write("Enter password: ");
        var password = Console.ReadLine();

        var user = new User { Username = username, Password = password };
        var createdUser = await _userRepository.AddAsync(user);
        
        Console.WriteLine($"User created with ID: {createdUser.Id}");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private async Task ListUsersAsync()
    {
        var users = _userRepository.GetMany().ToList();
        Console.WriteLine("ID\tUsername");
        foreach (var user in users)
        {
            Console.WriteLine($"{user.Id}\t{user.Username}");
        }
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}