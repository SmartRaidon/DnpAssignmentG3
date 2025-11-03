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
        return await response.Content.ReadFromJsonAsync<List<PostDto>>() ?? new List<PostDto>();
    }

    public async Task<PostDto?> GetPostByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PostDto>();
    }

    public async Task<PostDto> CreatePostAsync(PostCreateDto request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PostDto>() ?? throw new Exception("failed to create post");
    }

    public async Task UpdatePostAsync(int id, PostUpdateDto request)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeletePostAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
        response.EnsureSuccessStatusCode();
    }
}