using ApiContracts.DTO.Comment;

namespace BlazorApp.Services;

public interface ICommentService
{
    public Task<List<CommentDto>> GetCommentsByPostAsync(int postId, int? userId = null);
    public Task<CommentDto?> GetCommentByIdAsync(int postId, int commentId);
    public Task<CommentDto> CreateCommentAsync(int postId, CommentCreateDto request);
    public Task UpdateCommentAsync(int postId, int commentId, CommentUpdateDto request);
    public Task DeleteCommentAsync(int postId, int commentId);
}