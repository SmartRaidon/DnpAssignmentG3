using System.Text.Json;
using ApiContracts.DTO.User;

namespace BlazorApp.Services;

public class HttpUserService: IUserService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "api/users";

    public HttpUserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UserDto>> GetAllUsersAsync(string? username = null)
    {
        var url = string.IsNullOrWhiteSpace(username)
            ? BaseUrl
            : $"{BaseUrl}?username={Uri.EscapeDataString(username)}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<UserDto>>() ?? new List<UserDto>();
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UserDto>();
        
    }
    public async Task<UserDto> AddUserAsync(UserCreateDto request)
    {
        HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync(BaseUrl, request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<UserDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;

    }

    public async Task UpdateUserAsync(int id, UserUpdateDto request)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteUserAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
        response.EnsureSuccessStatusCode();
    }


}