using ApiContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task<PostDTO> AddPost(CreatePostDTO request);
    public Task UpdatePost(UpdatePostDTO request);
    public Task DeletePost(int id);
    public Task<PostDTO> GetSingle(int id);
    public Task<List<PostDTO>> GetMany();
}