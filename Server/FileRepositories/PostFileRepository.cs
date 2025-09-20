using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllTextAsync(filePath,"[]");
        }
    }

    private async Task<List<Post>> ReadFromJson()
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        return posts;
    }

    private async Task WriteToJson(List<Post> posts)
    {
        var postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
    }



    public async Task<Post> AddAsync(Post post)
    {
        var posts = await ReadFromJson();
        post.Id = posts.Any() ? posts.Max(p => p.Id) + 1 : 1; 
        posts.Add(post);
        await WriteToJson(posts);
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        var posts = await ReadFromJson();
        Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id); 
        if (existingPost is null) 
        { 
            throw new InvalidOperationException($"Post with ID '{post.Id}' not found"); 
        } 
        posts.Remove(existingPost);
        posts.Add(post);
        await WriteToJson(posts);


    }


    public async Task DeleteAsync(int id)
    {
        var posts = await ReadFromJson();
        Post? postToRemove = posts.SingleOrDefault(p => p.Id == id); 
        if (postToRemove is null) 
        { 
            throw new InvalidOperationException($"Post with ID '{id}' not found"); 
        } 
        posts.Remove(postToRemove);
        await WriteToJson(posts);

    }


    public async Task<Post> GetSingleAsync(int id)
    {
        var posts = await ReadFromJson();
        Post? postToRetrieve = posts.SingleOrDefault(p => p.Id == id); 
        if (postToRetrieve is null) 
        { 
            throw new InvalidOperationException($"Post with ID '{id}' not found"); 
        }

        return postToRetrieve;
    }
    
    public IQueryable<Post> GetMany()
    {
        string postsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        return posts.AsQueryable();

    }
    
    
}