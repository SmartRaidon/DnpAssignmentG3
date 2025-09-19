using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    private readonly User? currentUser;
    public SinglePostView(IPostRepository postRepository, IUserRepository userRepository, ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        currentUser = Session.CurrentUser;
    }

    public async Task ShowAsync(int postId)
    {
        var post = await postRepository.GetSingleAsync(postId);
        var author = await userRepository.GetSingleAsync(post.UserId);
        
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== [ TITLE: { post.Title } | AUTHOR: { author.Username }({ author.Id }) [ POST ID: {post.Id} ] ] ===");
            Console.WriteLine($"{ post.Body }");
        
            Console.WriteLine("=== [ COMMENTS ] ===");
            var comments = commentRepository.GetMany()
                .Where(c => c.PostId == post.Id);
            foreach (var comment in comments)
            {
                //var commentAuthor = await userRepository.GetSingleAsync(comment.UserId);
                Console.WriteLine($"[ {comment.Username} [ {comment.UserId} ]: {comment.Content}");
            }
        
            // listing available commands
            Console.WriteLine("\n[ CMDs ]");
            Console.WriteLine("/comment [message] - write a new comment");
            Console.WriteLine("/back - go back to posts");
            
            Console.Write("\n> ");
            string? input = Console.ReadLine();

            if (input == "/back") break;

            if (input != null && input.StartsWith("/comment "))
            {
                string text = input.Substring(9).Trim(); // removing the '/comment ' part - 9 chars total

                if (!string.IsNullOrWhiteSpace(text))
                {
                    await AddCommentAsync(postId, text);
                    // reload post and author to get fresh data
                    post = await postRepository.GetSingleAsync(postId);
                    author = await userRepository.GetSingleAsync(post.UserId);
                }
                else
                {
                    Console.WriteLine("Comment cannot be empty!");
                    Console.ReadKey();
                }
            }
        }
    }

    private async Task<Comment> AddCommentAsync(int postId, string content)
    {
        var newComment = new Comment
        {
            PostId = postId,
            Content = content,
            Username = currentUser.Username,
            UserId = currentUser.Id
        };
        
        Comment created = await commentRepository.AddAsync(newComment);
        Console.WriteLine($"Comment created with ID: {created.Id}");
        return created;
    }
}