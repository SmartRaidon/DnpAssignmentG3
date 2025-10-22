using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePost;

public class CreatePostView
{
    private IPostRepository _postRepository;

    public CreatePostView(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task AddPostAsync(int userId, string title, string body)
    {
        Post userToAdd = new Post
        {
            UserId = userId,
            Title = title,
            Body = body
        };
        
        Post createdPost = await _postRepository.AddAsync(userToAdd);
        Console.WriteLine($"Post created with details: {createdPost}");
    }
}