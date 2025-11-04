using System.Net;
using System.Security.Claims;
using System.Text.Json;
using ApiContracts;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp.Auth;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private ClaimsPrincipal _principal;

    public SimpleAuthProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task Login(string username, string password)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Auth/login",
            new CreateUserDto { Username = username, Password = password });
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);    
        }
        UserDto userDto = JsonSerializer.Deserialize<UserDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, userDto.UserName),
            new Claim("Id", userDto.Id.ToString())
            // here we can add e-mail as well
        };
        
        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");
        _principal = new ClaimsPrincipal(identity);
        
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_principal)));
    }

    public void Logout()
    {
        _principal = new ();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_principal)));
    }
    
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(_principal ?? new ()));
    }
}