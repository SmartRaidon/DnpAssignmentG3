using ApiContracts.DTO.Post;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task<List<PostDto>> GetAllPostsAsync(string? title = null, int? userId = null);
    public Task<PostDto?> GetPostByIdAsync(int id);
    public Task<PostDto> CreatePostAsync(PostCreateDto request);
    public Task UpdatePostAsync(int id, PostUpdateDto request);
    public Task DeletePostAsync(int id);

}