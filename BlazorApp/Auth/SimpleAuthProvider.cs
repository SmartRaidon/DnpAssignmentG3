using System.Security.Claims;
using System.Text.Json;
using ApiContracts;
using Microsoft.AspNetCore.Components.Authorization;
using LoginRequest = ApiContracts.Authentication.LoginRequest;

namespace BlazorApp.Auth;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private ClaimsPrincipal currentClaimsPrincipal;

    public SimpleAuthProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task Login(LoginRequest request)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("auth/login", request);

        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        UserDTO userDto = JsonSerializer.Deserialize<UserDTO>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        //build claims
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, userDto.Username),
            new Claim("Id", userDto.Id.ToString())
        };
        
        //identity + principal
        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");
        currentClaimsPrincipal = new ClaimsPrincipal(identity);
        
        //notice Blazor of changed auth state
        NotifyAuthenticationStateChanged
            (Task.FromResult(new AuthenticationState(currentClaimsPrincipal))
        );
    }
    //reset ClaimsPrincipal to empty one and notify framework about the change in auth state
    public async Task Logout()
    {
        currentClaimsPrincipal = new();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentClaimsPrincipal)));
    }
    
    //method called by Blazor framework to access current auth state
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(currentClaimsPrincipal ?? new()));
        // '??' checks id currentClaimsPrincipal is null, and if so returns prt after the ??
    }
}