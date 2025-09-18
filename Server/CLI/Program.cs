using CLI.UI;
using CLI.UI.Login;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using Entities;
using InMemoryRepositories;
using RepositoryContracts;

Console.WriteLine("Starting CLI app...");
IUserRepository userRepository = new UserInMemoryRepository();
ICommentRepository commentRepository = new CommentInMemoryRepository();
IPostRepository postRepository = new PostInMemoryRepository();
LoginView loginView = new(userRepository);
CreateUserView createUserView = new(userRepository);

while (Session.CurrentUser == null)
{
    Console.Clear();
    Console.WriteLine("=== Welcome to the Forum ===");
    Console.WriteLine("1. Login");
    Console.WriteLine("2. Register");
    Console.WriteLine("3. Tester");
    Console.WriteLine("0. Exit");
    Console.Write("Choose: ");

    string? choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            await loginView.ShowAsync();
            break;
        case "2":
            await createUserView.ShowAsync();
            break;
        case "3":
            var testUser = new User()
            {
                Id = 999,
                Username = "Tester",
                Password = "Tester"
            };
            Session.CurrentUser = testUser;
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Invalid option!");
            Console.ReadKey();
            break;
    }
}

CliApp cliApp = new(userRepository, commentRepository, postRepository);
await cliApp.StartAsync();