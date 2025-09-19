using Entities;
using RepositoryContracts;

namespace CLI.UI.Login;

public class LoginView
{
    private readonly IUserRepository userRepository;
    private readonly User? currentUser;

    public LoginView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        Console.Clear();
        Console.WriteLine("=== [ LOGIN ] ===");
        
        Console.Write("Username: ");
        string? username = Console.ReadLine();
        
        Console.Write("Password: ");
        string? password = Console.ReadLine();
        
        var user = userRepository.GetMany()
            .FirstOrDefault(u => u.Username == username && u.Password == password);
        
        if (user != null)
        {
            Session.CurrentUser = user;
            Console.WriteLine($"Welcome, {user.Username}!");
        }
        else
        {
            Console.WriteLine("Invalid username or password.");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}