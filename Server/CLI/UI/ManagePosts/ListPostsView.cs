using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView
{
    private readonly IPostRepository postRepository;

    public ListPostsView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }
    
    public async Task ShowAsync()
    {
        Console.Clear();
        Console.WriteLine("=== [ POSTS ] ===");

        // this should be async later when we have database
        ListPosts();
    }

    private void ListPosts()
    {
        var posts = postRepository.GetMany();
        foreach (var post in posts)
        {
            Console.WriteLine($"[{post.Id}] {post.Title}");
        }
    }
}