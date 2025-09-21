using System.Text.Json;

namespace FileRepositories;

using Entities;
using RepositoryContracts;

public class UserFileRepository : IUserRepository
{
    private readonly  string _filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }
    
    public async Task<User> AddAsync(User user)
    {
        string usersAsJson = await File.ReadAllTextAsync(_filePath); 
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!; 
        int maxId = users.Count > 0 ? users.Max(c => c.Id) : 0; 
        user.Id = maxId + 1; 
        users.Add(user); 
        usersAsJson = JsonSerializer.Serialize(users); 
        await File.WriteAllTextAsync(_filePath, usersAsJson); 
        return user; 
    }

    public async Task UpdateAsync(User user)
    {
        string usersAsJson = await File.ReadAllTextAsync(_filePath); 
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser == null)
        {
            throw new InvalidOperationException($"User with ID: {user.Id} not found.");
        }
        existingUser.Id = user.Id;
        existingUser.Username = user.Username;
        existingUser.Password = user.Password;
        usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(_filePath, usersAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        string usersAsJson = await File.ReadAllTextAsync(_filePath); 
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        var existingUser = users.FirstOrDefault(u => u.Id == id);
        if (existingUser == null)
        {
            return;
        }
        users.Remove(existingUser);
        usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(_filePath, usersAsJson);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        string usersAsJson = await File.ReadAllTextAsync(_filePath); 
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        var existingUser = users.FirstOrDefault(u => u.Id == id);
        if (existingUser == null)
        {
            throw new InvalidOperationException($"User with ID: {id} not found.");
        }
        return existingUser;
    }

    public IQueryable<User> GetMany()
    {
        string usersAsJson = File.ReadAllTextAsync(_filePath).Result; 
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!; 
        return users.AsQueryable(); 
    }
}