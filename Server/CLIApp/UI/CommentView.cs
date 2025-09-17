using RepositoryContracts;
using Entities;

namespace CLI.UI;

public class CommentView
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;

    public CommentView(ICommentRepository commentRepository, IUserRepository userRepository, IPostRepository postRepository)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
        _postRepository = postRepository;
    }

    public async Task ShowCommentManagement()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Comment Management ===");
            Console.WriteLine("1. Add Comment");
            Console.WriteLine("2. Back to Main Menu");
            Console.Write("Select an option: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await AddCommentAsync();
                    break;
                case "2":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task AddCommentAsync()
    {
        Console.Write("Enter post ID: ");
        if (!int.TryParse(Console.ReadLine(), out int postId))
        {
            Console.WriteLine("Invalid post ID.");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter user ID: ");
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid user ID.");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter comment content: ");
        var content = Console.ReadLine();

        var comment = new Comment { PostId = postId, UserId = userId, Content = content };
        var createdComment = await _commentRepository.AddAsync(comment);
        
        Console.WriteLine($"Comment created with ID: {createdComment.Id}");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}