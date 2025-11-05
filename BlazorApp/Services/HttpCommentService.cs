using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient _httpClient;

    public HttpCommentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<CommentDto> AddCommentAsync(CommentDto request)
    {
        HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync("comments", request); // api/users maybe maybe
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<CommentDto> GetCommentAsync(int id)
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync($"comments/{id}");
        string response = httpResponse.Content.ReadAsStringAsync().Result;
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<List<CommentDto>> GetCommentsByPostIdAsync(int postId)
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync("comments/post/{postId}");
        string response = httpResponse.Content.ReadAsStringAsync().Result;
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<List<CommentDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task UpdateCommentAsync(int id, CommentDto request)
    {
        HttpResponseMessage httpResponse = await _httpClient.PutAsJsonAsync($"comments/{id}", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }

    public async Task DeleteCommentAsync(int id)
    {
        HttpResponseMessage httpResponse = await _httpClient.DeleteAsync($"comments/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }
}