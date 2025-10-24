using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository postRepository;
    private readonly User? currentUser;

    public CreatePostView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
        currentUser = Session.CurrentUser;
    }

    public async Task ShowAsync()
    {
        Console.Clear();
        Console.WriteLine("=== [ CREATE NEW POST ] ===");
        
        Console.WriteLine("\nTitle of post: ");
        Console.Write("> ");
        string? title = Console.ReadLine();
        
        Console.WriteLine("\nBody of post: ");
        Console.Write("> ");
        string? body = Console.ReadLine();

        if (title != null && body != null && currentUser != null)
        {
            await AddPostAsync(title, body, currentUser);
        }
        else
        {
            Console.WriteLine("You need to enter a title and a body to create your post!");
        }
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
    
    private async Task<Post> AddPostAsync(string title, string body, User creator)
    {
        Post post = new()
        {
            Body = body,
            Title = title,
            UserId = creator.Id
        };
        
        Post created = await postRepository.AddAsync(post);
        Console.WriteLine($"Post created: {created.Id}");
        return created;
    }
}