using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePost;

public class UpdatePostView
{
    private IPostRepository _postRepository;

    public UpdatePostView(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task UpdatePostAsync(int id, Post postToUpdate)
    {
        if(string.IsNullOrWhiteSpace(postToUpdate.Title) || string.IsNullOrWhiteSpace(postToUpdate.Title))
        {
            throw new ArgumentNullException(nameof(postToUpdate.Title),"Cannot be null or empty");
        }
        if(string.IsNullOrWhiteSpace(postToUpdate.Body) || string.IsNullOrWhiteSpace(postToUpdate.Body))
        {
            throw new ArgumentNullException(nameof(postToUpdate.Body),"Cannot be null or empty");
        } 
        
        Post existingPost = await _postRepository.GetSingleAsync(id);
        
        existingPost.UserId = postToUpdate.UserId;
        existingPost.Title = postToUpdate.Title;
        existingPost.Body = postToUpdate.Body;
        
        await _postRepository.UpdateAsync(existingPost);
        Console.WriteLine($"Post updated: {existingPost.Title}");
        
        await Task.CompletedTask;
    }
}