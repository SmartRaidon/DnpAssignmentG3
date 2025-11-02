using ApiContracts.DTO.Comment;

namespace BlazorApp.Services;

public class HttpCommentService:ICommentService
{
    private readonly HttpClient _httpClient;
    
    public HttpCommentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<List<CommentDto>> GetCommentsByPostAsync(int postId, int? userId = null)
    {
        var url = userId.HasValue
            ? $"api/posts/{postId}/comments?userId={userId.Value}" 
            : $"api/posts/{postId}/comments";
        var response = await _httpClient.GetAsync(url);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return new List<CommentDto>();
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<CommentDto>>() ?? new List<CommentDto>();
    }

    public async Task<CommentDto?> GetCommentByIdAsync(int postId, int commentId)
    {
        var response = await _httpClient.GetAsync($"api/posts/{postId}/comments/{commentId}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CommentDto>();
    }

    public async Task<CommentDto> CreateCommentAsync(int postId, CommentCreateDto request)
    {
       var response = await _httpClient.PostAsJsonAsync($"api/posts/{postId}/comments", request);
       response.EnsureSuccessStatusCode();
       return await response.Content.ReadFromJsonAsync<CommentDto>() ?? throw new Exception("Failed to create comment");
       
    }

    public async Task UpdateCommentAsync(int postId, int commentId, CommentUpdateDto request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/posts/{postId}/comments/{commentId}", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteCommentAsync(int postId, int commentId)
    {
        var response = await _httpClient.DeleteAsync($"api/posts/{postId}/comments/{commentId}");
        response.EnsureSuccessStatusCode();
    }
}