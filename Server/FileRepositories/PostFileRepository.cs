using System.Runtime.InteropServices;
using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private static readonly string BASE_PATH =
        Path.Combine(AppContext.BaseDirectory, @"..\..\..\Files");
    private static readonly string PATH =
        Path.GetFullPath(Path.Combine(BASE_PATH, "posts.json"));

    public PostFileRepository()
    {
        InitializeFileIFNotExists();
    }
    public async Task<Post> AddAsync(Post post)
    {
        string jsonPostList = await File.ReadAllTextAsync(PATH);
        
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(jsonPostList);
        
        post.Id = posts.Any() ? posts.Max(p => p.Id) + 1 : 1;
        posts.Add(post);
        
        jsonPostList = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(PATH, jsonPostList);
        
        return await Task.FromResult(post);
    }

    public async Task UpdateAsync(Post post)
    {
        string jsonPostList = await File.ReadAllTextAsync(PATH);
        
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(jsonPostList);
        
        Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException($"Post {post.Id} not found");
        }
        
        posts.Remove(existingPost);
        posts.Add(post);
        
        jsonPostList = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(PATH, jsonPostList);
    }

    public async Task DeleteAsync(int id)
    {
        string  jsonPostList = await File.ReadAllTextAsync(PATH);
        
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(jsonPostList);
        
        Post? postToRemove = posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException($"Post {id} not found");
        }
        
        posts.Remove(postToRemove);
        
        jsonPostList = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(PATH, jsonPostList);
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        string  jsonPostList = await File.ReadAllTextAsync(PATH);
        
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(jsonPostList);
        
        Post? postToGet = posts.SingleOrDefault(p => p.Id == id);
        if (postToGet is null)
        {
            throw new InvalidOperationException($"Post {id} not found");
        }
        
        return await Task.FromResult(postToGet);
    }

    public async Task<IQueryable<Post>> GetManyAsync()
    {
        string  jsonPostList = await File.ReadAllTextAsync(PATH);
        
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(jsonPostList);
        
        return await Task.FromResult(posts.AsQueryable());
    }

    private async void InitializeFileIFNotExists()
    {
        Directory.CreateDirectory(BASE_PATH);
        
        bool fileAlreadyExists = File.Exists(PATH);
        if (!fileAlreadyExists)
        {
            List<Post> posts = new List<Post>();
            string jsonPosts = JsonSerializer.Serialize(posts);
            await File.WriteAllTextAsync(PATH, jsonPosts);
        }
    }
}