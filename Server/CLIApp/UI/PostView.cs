using RepositoryContracts;
using Entities;

namespace CLI.UI;

public class PostView
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICommentRepository _commentRepository;

    public PostView(IPostRepository postRepository, IUserRepository userRepository, ICommentRepository commentRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _commentRepository = commentRepository;
    }

    public async Task ShowPostManagement()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Post Management ===");
            Console.WriteLine("1. Create Post");
            Console.WriteLine("2. List Posts (Overview)");
            Console.WriteLine("3. View Specific Post");
            Console.WriteLine("4. Back to Main Menu");
            Console.Write("Select an option: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await CreatePostAsync();
                    break;
                case "2":
                    await ListPostsOverviewAsync();
                    break;
                case "3":
                    await ViewSpecificPostAsync();
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

    private async Task CreatePostAsync()
    {
        Console.Write("Enter title: ");
        var title = Console.ReadLine();
        Console.Write("Enter body: ");
        var body = Console.ReadLine();
        Console.Write("Enter user ID: ");
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid user ID.");
            Console.ReadKey();
            return;
        }

        var post = new Post { Title = title, Body = body, UserId = userId };
        var createdPost = await _postRepository.AddAsync(post);
        
        Console.WriteLine($"Post created with ID: {createdPost.Id}");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private async Task ListPostsOverviewAsync()
    {
        var posts = _postRepository.GetMany().ToList();
        Console.WriteLine("ID\tTitle");
        foreach (var post in posts)
        {
            Console.WriteLine($"{post.Id}\t{post.Title}");
        }
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private async Task ViewSpecificPostAsync()
    {
        Console.Write("Enter post ID: ");
        if (!int.TryParse(Console.ReadLine(), out int postId))
        {
            Console.WriteLine("Invalid post ID.");
            Console.ReadKey();
            return;
        }

        try
        {
            var post = await _postRepository.GetSingleAsync(postId);
            Console.WriteLine($"Title: {post.Title}");
            Console.WriteLine($"Body: {post.Body}");
            Console.WriteLine("Comments:");

            var comments = _commentRepository.GetMany().Where(c => c.PostId == postId).ToList();
            foreach (var comment in comments)
            {
                var user = await _userRepository.GetSingleAsync(comment.UserId);
                Console.WriteLine($"- {user.Username}: {comment.Content}");
            }
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("Post not found.");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}