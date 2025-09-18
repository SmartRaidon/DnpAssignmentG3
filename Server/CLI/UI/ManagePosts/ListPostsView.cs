using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePost;

public class ListPostsView
{
    private IPostRepository _postRepository;
    
    public ListPostsView(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task getManyAsync()
    {
        IQueryable<Post> posts = _postRepository.GetMany();
        Console.WriteLine("Posts:");
        foreach (var post in posts)
        {
            Console.WriteLine(post);
        }
        await Task.CompletedTask;
    }
}