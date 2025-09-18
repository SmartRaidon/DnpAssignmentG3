using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePost;

public class SinglePostView
{
    private IPostRepository _postRepository;
    public SinglePostView(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }
    
    public async Task GetSingleAsync(int id)
    {
        Post postToGet = await _postRepository.GetSingleAsync(id);
        Console.WriteLine($"Current post: {postToGet}");
       
        await Task.CompletedTask;
    }
}