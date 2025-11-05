using ApiContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task<PostDto> AddPostAsync(PostDto request);
    public Task<PostDto> GetPostAsync(int id);
    public Task<List<PostDto>> GetPostsAsync();
    public Task UpdatePostAsync(int id, PostDto request);
    public Task DeletePostAsync(int id);
}