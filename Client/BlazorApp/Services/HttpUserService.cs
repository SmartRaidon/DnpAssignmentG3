using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiContracts;
using BlazorApp.Http.Interfaces;

namespace BlazorApp.Http.Services
{
    public class HttpUserService : IUserService
    {
        private readonly HttpClient client;

        public HttpUserService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<UserDto> AddUserAsync(CreateUserDto request)
        {
            HttpResponseMessage httpResponse = await client.PostAsJsonAsync("users", request);
            string response = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception(response);

            return JsonSerializer.Deserialize<UserDto>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            return await client.GetFromJsonAsync<UserDto>($"users/{id}");
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return await client.GetFromJsonAsync<IEnumerable<UserDto>>("users") ?? new List<UserDto>();
        }

       /* public async Task UpdateUserAsync(int id, UpdateUserDto request)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync($"users/{id}", request);
            if (!response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                throw new Exception(content);
            }
        }
*/
        public async Task DeleteUserAsync(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"users/{id}");
            if (!response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                throw new Exception(content);
            }
        }
    }
}
