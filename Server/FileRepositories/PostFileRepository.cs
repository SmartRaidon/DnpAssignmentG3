using System.Text.Json;

namespace FileRepositories;

using Entities;
using RepositoryContracts;

public class PostFileRepository : IPostRepository
{
    private readonly string _filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }
    
    public async Task<Post> AddAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(_filePath); 
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!; 
        int maxId = posts.Count > 0 ? posts.Max(c => c.Id) : 0; 
        post.Id = maxId + 1; 
        posts.Add(post); 
        postsAsJson = JsonSerializer.Serialize(posts); 
        await File.WriteAllTextAsync(_filePath, postsAsJson); 
        return post; 
    }

    public async Task UpdateAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(_filePath); 
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        var existingPost = posts.FirstOrDefault(p => p.Id == post.Id);
        if (existingPost == null)
        {
            throw new InvalidOperationException($"Post with ID: {post.Id} not found.");
        }
        existingPost.UserId = post.UserId;
        existingPost.Id = post.Id;
        existingPost.Title = post.Title;
        existingPost.Body = post.Body; 
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(_filePath, postsAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        string postsAsJson = await File.ReadAllTextAsync(_filePath); 
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        var existingPost = posts.FirstOrDefault(p => p.Id == id);
        if (existingPost == null)
        {
            return;
        }
        posts.Remove(existingPost);
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(_filePath, postsAsJson);
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        string postsAsJson = await File.ReadAllTextAsync(_filePath); 
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        var existingPost = posts.FirstOrDefault(p => p.Id == id);
        if (existingPost == null)
        {
            throw new InvalidOperationException($"Post with ID: {id} not found.");
        }
        return existingPost;
    }

    public IQueryable<Post> GetMany()
    {
        string postsAsJson = File.ReadAllTextAsync(_filePath).Result; 
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!; 
        return posts.AsQueryable(); 
    }
}