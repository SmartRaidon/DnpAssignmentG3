using System.Text.Json;

namespace FileRepositories;

using Entities;
using RepositoryContracts;

public class CommentFileRepository : ICommentRepository
{
    private readonly string _filePath = "comments.json"; 
    
    public CommentFileRepository() 
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    } 
    
    public async Task<Comment> AddAsync(Comment comment)
    {
        string commentsAsJson = await File.ReadAllTextAsync(_filePath); 
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!; 
        int maxId = comments.Count > 0 ? comments.Max(c => c.Id) : 0; 
        comment.Id = maxId + 1; 
        comments.Add(comment); 
        var options = new JsonSerializerOptions { WriteIndented = true }; // make it pretty
        commentsAsJson = JsonSerializer.Serialize(comments, options); 
        await File.WriteAllTextAsync(_filePath, commentsAsJson); 
        return comment; 
    }

    public async Task UpdateAsync(Comment comment)
    {
        string commentsAsJson = await File.ReadAllTextAsync(_filePath); 
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        var existingComment = comments.FirstOrDefault(c => c.Id == comment.Id);
        if (existingComment == null)
        {
            throw new InvalidOperationException($"Comment with ID: {comment.Id} not found.");
        }
        existingComment.UserId = comment.UserId;
        existingComment.Username = comment.Username;
        existingComment.Content = comment.Content;
        existingComment.PostId = comment.PostId; 
        existingComment.Id = comment.Id;
        var options = new JsonSerializerOptions { WriteIndented = true }; // make it pretty
        commentsAsJson = JsonSerializer.Serialize(comments, options);
        await File.WriteAllTextAsync(_filePath, commentsAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        string commentsAsJson = await File.ReadAllTextAsync(_filePath); 
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        var existingComment = comments.FirstOrDefault(c => c.Id == id);
        if (existingComment == null)
        {
            return; // we don't do anything
        }
        comments.Remove(existingComment);
        var options = new JsonSerializerOptions { WriteIndented = true }; // make it pretty
        commentsAsJson = JsonSerializer.Serialize(comments, options);
        await File.WriteAllTextAsync(_filePath, commentsAsJson);
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        string commentsAsJson = await File.ReadAllTextAsync(_filePath); 
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        var existingComment = comments.FirstOrDefault(c => c.Id == id);
        if (existingComment == null)
        {
            throw new InvalidOperationException($"Comment with ID: {id} not found.");
        }
        return existingComment;
    }

    public IQueryable<Comment> GetMany()
    {
        string commentsAsJson = File.ReadAllTextAsync(_filePath).Result; 
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!; 
        return comments.AsQueryable(); 
    }
}