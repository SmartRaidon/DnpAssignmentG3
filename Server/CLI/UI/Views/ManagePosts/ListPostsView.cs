using Entities;
using RepositoryContracts;

namespace CLI.UI.Views.ManagePost;

public class ListPostsView
{
    private IPostRepository _postRepository;
    
    public ListPostsView(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task GetManyAsync()
    {
        IQueryable<Post> posts = await _postRepository.GetManyAsync();

        if (!posts.Any())
        {
            Console.WriteLine("No posts found");
        }
        else
        {
            Console.WriteLine("Posts:");
            foreach (var post in posts) 
            {
                Console.WriteLine(post);   
            }
        }
        await Task.CompletedTask;
    }
}