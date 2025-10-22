using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private static readonly string BASE_PATH =
        Path.Combine(AppContext.BaseDirectory, @"..\..\..\Files");
    private static readonly string PATH =
        Path.GetFullPath(Path.Combine(BASE_PATH, "users.json"));

    public UserFileRepository()
    {
        InitializeFileIfNotExists();
    }
    public async Task<User> AddAsync(User user)
    {
        string jsonUserList = await File.ReadAllTextAsync(PATH);
        
        List<User> users = JsonSerializer.Deserialize<List<User>>(jsonUserList);
        
        user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
        users.Add(user);
        
        jsonUserList = JsonSerializer.Serialize(users); 
        await File.WriteAllTextAsync(PATH, jsonUserList);
        
        return await Task.FromResult(user);
    }

    public async Task UpdateAsync(User user)
    {
        string jsonUserList = await File.ReadAllTextAsync(PATH);
        
        List<User> users = JsonSerializer.Deserialize<List<User>>(jsonUserList);
        
        User? existingUser = users.SingleOrDefault(u => u.Id == user.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException($"User with id {user.Id} not found");
        }

        users.Remove(existingUser);
        users.Add(user);
        
        jsonUserList = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(PATH, jsonUserList);
    }

    public async Task DeleteAsync(int id)
    {
        string jsonUserList = await File.ReadAllTextAsync(PATH);
        
        List<User> users = JsonSerializer.Deserialize<List<User>>(jsonUserList);
        
        User? userToRemove = users.SingleOrDefault(u => u.Id == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException($"User with id {id} not found");
        }
        
        users.Remove(userToRemove);
        
        jsonUserList = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(PATH, jsonUserList);
    }

    public async Task<User?> GetSingleAsync(int id)
    {
        string  jsonUserList = await File.ReadAllTextAsync(PATH);
        
        List<User> users = JsonSerializer.Deserialize<List<User>>(jsonUserList);
        
        User? userToGet = users.SingleOrDefault(u => u.Id == id, null);
        
        return await Task.FromResult(userToGet);
    }

    public async Task<IQueryable<User>> GetManyAsync()
    {
        string  jsonUserList = await File.ReadAllTextAsync(PATH);
        
        List<User> users = JsonSerializer.Deserialize<List<User>>(jsonUserList);

        return await Task.FromResult(users.AsQueryable());
    }

    private async void InitializeFileIfNotExists()
    {
        Directory.CreateDirectory(BASE_PATH);
        
        bool fileAlreadyExists = File.Exists(PATH);
        if (!fileAlreadyExists)
        {
            List<User> users = new List<User>();
            string jsonComments = JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(PATH, jsonComments);
        }
    }
}