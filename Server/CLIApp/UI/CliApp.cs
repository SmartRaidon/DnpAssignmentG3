using RepositoryContracts;
using Entities;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly UserView _userView;
    private readonly PostView _postView;
    private readonly CommentView _commentView;

    public CliApp(IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository)
    {
        _userRepository = userRepository;
        _postRepository = postRepository;
        _commentRepository = commentRepository;
        _userView = new UserView(_userRepository);
        _postView = new PostView(_postRepository, _userRepository, _commentRepository);
        _commentView = new CommentView(_commentRepository, _userRepository, _postRepository);
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Blog Management System ===");
            Console.WriteLine("1. User Management");
            Console.WriteLine("2. Post Management");
            Console.WriteLine("3. Comment Management");
            Console.WriteLine("4. Exit");
            Console.Write("Select an option: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await _userView.ShowUserManagement();
                    break;
                case "2":
                    await _postView.ShowPostManagement();
                    break;
                case "3":
                    await _commentView.ShowCommentManagement();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}