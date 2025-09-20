using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository: IUserRepository
{
    private readonly string filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllTextAsync(filePath,"[]");
        }
    }

    private async Task<List<User>> ReadFromJson()
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        return users;
    }

    private async Task WriteToJson(List<User> users)
    {
        var usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, usersAsJson);
    }

    public async Task<User> AddAsync(User user)
    {
        var users =  await ReadFromJson();
        user.Id = users.Any() ? users.Max(p => p.Id) + 1 : 1; 
        users.Add(user);
        await WriteToJson(users);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        var users = await ReadFromJson();
        User? existingUser = users.SingleOrDefault(p => p.Id == user.Id); 
        if (existingUser is null) 
        { 
            throw new InvalidOperationException($"User with ID '{user.Id}' not found"); 
        } 
        users.Remove(existingUser);
        users.Add(user);
        await WriteToJson(users);
    }

    public async Task DeleteAsync(int id)
    {
        var users = await ReadFromJson();
        User? userToRemove = users.SingleOrDefault(p => p.Id == id); 
        if (userToRemove is null) 
        { 
            throw new InvalidOperationException($"User with ID '{id}' not found"); 
        } 
        users.Remove(userToRemove);
        await WriteToJson(users);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        var users = await ReadFromJson();
        User? userToRetrieve = users.SingleOrDefault(p => p.Id == id); 
        if (userToRetrieve is null) 
        { 
            throw new InvalidOperationException($"User with ID '{id}' not found"); 
        }

        return userToRetrieve;
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        var users = await ReadFromJson();
        User? userToRetrieve = users.SingleOrDefault(p => p.Username == username);
        if (userToRetrieve is null)
        {
            throw new InvalidOperationException($"User with username '{username}' not found");
        }

        return userToRetrieve;
    }
    public IQueryable<User> GetMany()
    {
        string usersAsJson = File.ReadAllTextAsync(filePath).Result;
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

        return users.AsQueryable();
    }
    

}