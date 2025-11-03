using System.Text.Json;
using ApiContracts.DTO.Post;

namespace BlazorApp.Services;

public class HttpPostService: IPostService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "api/posts";

    public HttpPostService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<PostDto>> GetAllPostsAsync(string? title = null, int? userId = null)
    {
        var queryParameters = new List<String>();
        if(!string.IsNullOrWhiteSpace(title)) queryParameters.Add($"title={Uri.EscapeDataString(title)}");
        if (userId.HasValue) queryParameters.Add($"userId={userId.Value}");
        var url = queryParameters.Any() ? $"{BaseUrl}?{string.Join("&", queryParameters)}" : BaseUrl;
        Console.WriteLine(url);
        var response = await _httpClient.GetAsync(url);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return new List<PostDto>();
        response.EnsureSuccessStatusCode();
        var payload = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<PostDto>>(payload, new JsonSerializerOptions
        {
        PropertyNameCaseInsensitive = true
        }) ?? new List<PostDto>();
    }

    public async Task<PostDto?> GetPostByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        response.EnsureSuccessStatusCode();
        var payload = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PostDto>(payload, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<PostDto> CreatePostAsync(PostCreateDto request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}", request);
        var payload = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) throw new Exception(payload);
        return JsonSerializer.Deserialize<PostDto>(payload, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task UpdatePostAsync(int id, PostUpdateDto request)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", request);
        var payload = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) throw new Exception(payload);
    }

    public async Task DeletePostAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
        var payload = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) throw new Exception(payload);
    }
}