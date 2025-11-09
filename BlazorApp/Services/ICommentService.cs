using ApiContracts;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace BlazorApp.Services;

public interface ICommentService
{
    public Task<CommentDTO> AddComment(CreateCommentDTO request);
    public Task UpdateComment(UpdateCommentDTO request);
    public Task DeleteComment(int id);
    public Task<CommentDTO> GetSingle(int id);
    public Task<List<CommentDTO>> GetMany();
    
}