using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepository;

    public CreateUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        Console.Clear();
        Console.WriteLine("=== [ REGISTER NEW USER ] ===");

        Console.Write("Enter Username: ");
        string? username = Console.ReadLine();

        Console.Write("Enter Password: ");
        string? password = Console.ReadLine();

        if (username != null && password != null)
        {
            if (Check(username, password))
            {
                await AddUserAsync(username, password); 
            }
            Console.WriteLine("User registered successfully!");
        }
        else
        {
            Console.WriteLine("Error! Registration aborted!");
        }
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private static bool Check(string name, string password)
    {
        if (name.Length < 3)
        {
            Console.WriteLine("Name is too short. [ Less than 3 characters ]");
            return false;
        }

        if (password.Length < 4)
        {
            Console.WriteLine("Password is too short. [ Less than 4 characters ]");
            return false;
        }
        
        if (name.Contains(' ') && password.Contains(' '))
        {
            Console.WriteLine("Username or password contains spaces.");
            return false;
        }
        return true;
    }
    
    private async Task<User> AddUserAsync(string name, string password)
    {
        User user = new()
        {
            Username = name,
            Password = password
        };
        
        User created = await userRepository.AddAsync(user);
        Console.WriteLine($"User created: {created.Id}");
        return created;
    }
}