using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient _httpClient;

    public HttpPostService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<PostDto> AddPostAsync(PostDto request)
    {
        HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync("posts/add", request); // api/users maybe maybe
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<PostDto> GetPostAsync(int id)
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync($"posts/{id}");
        string response = httpResponse.Content.ReadAsStringAsync().Result;
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<List<PostDto>> GetPostsAsync()
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync("posts");
        string response = httpResponse.Content.ReadAsStringAsync().Result;
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<List<PostDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task UpdatePostAsync(int id, PostDto request)
    {
        HttpResponseMessage httpResponse = await _httpClient.PutAsJsonAsync($"posts/{id}", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }

    public async Task DeletePostAsync(int id)
    {
        HttpResponseMessage httpResponse = await _httpClient.DeleteAsync($"posts/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }
}