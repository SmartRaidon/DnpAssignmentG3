using RepositoryContracts;
using CLI.UI.ManagePosts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;
    private readonly ListPostsView listPostsView;

    public CliApp(IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        listPostsView = new ListPostsView(postRepository);
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            
            // printing existing posts
            await listPostsView.ShowAsync();
            
            // listing available commands
            Console.WriteLine("\n[ CMDs ]");
            Console.WriteLine("/create - create a new post");
            Console.WriteLine("/open [ID] - open post");
            Console.WriteLine("/exit - quitting the application");
            
            // user input
            Console.Write("> ");
            string? input = Console.ReadLine();
            
            // empty input check
            if (string.IsNullOrWhiteSpace(input))
                continue;
            
            var parts = input.Split(' ', 2); // 1: command 2: parameter
            var command = parts[0].ToLower();
            var argument = parts.Length > 1 ? parts[1] : null;

            switch (command)
            {
                case "/create":
                    var createView = new CreatePostView(postRepository);
                    await createView.ShowAsync();
                    break;
                case  "/open":
                    if (int.TryParse(argument, out int postId))
                    {
                        var singlePostView = new SinglePostView(postRepository, userRepository, commentRepository);
                        await singlePostView.ShowAsync(postId);
                    }
                    else Console.WriteLine("Invalid post ID!");
                    Console.ReadKey();
                    break;
                case "/exit":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Unknown command!");
                    Console.ReadKey();
                    break;
            }
            
        }
    }
}