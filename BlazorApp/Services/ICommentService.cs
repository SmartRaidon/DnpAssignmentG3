using ApiContracts;

namespace BlazorApp.Services;

public interface ICommentService
{
    public Task<CommentDto> AddCommentAsync(CommentDto request);
    public Task<CommentDto> GetCommentAsync(int id);
    public Task<List<CommentDto>> GetCommentsByPostIdAsync(int postId);
    public Task UpdateCommentAsync(int id, CommentDto request);
    public Task DeleteCommentAsync(int id);
}