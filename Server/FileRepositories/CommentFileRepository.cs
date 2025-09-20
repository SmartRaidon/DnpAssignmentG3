using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private readonly string filePath = "commnets.json";

    public CommentFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllTextAsync(filePath,"[]");
        }
    }

    private async Task<List<Comment>> ReadFromJson()
    {
	    string commentsAsJson = await File.ReadAllTextAsync(filePath);
	    List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
	    return comments;
    }

    private async Task WriteToJson(List<Comment> comments)
    {
	    var commentsAsJson = JsonSerializer.Serialize(comments);
	    await File.WriteAllTextAsync(filePath, commentsAsJson);
    }
    
    public async Task<Comment> AddAsync(Comment comment)
    {
	    var comments = await ReadFromJson();
	    int maxId = comments.Count > 0 ? comments.Max(c => c.Id) : 1;
	    comment.Id = maxId + 1;
	    comments.Add(comment);
	    await WriteToJson(comments);
	    return comment;
	}

    public async Task UpdateAsync(Comment comment)
    {
	    var comments = await ReadFromJson();
	    Comment? existingComment = comments.SingleOrDefault(p => p.Id == comment.Id); 
	    if (existingComment is null) 
	    { 
		    throw new InvalidOperationException($"Comment with ID '{comment.Id}' not found"); 
	    } 
	    comments.Remove(existingComment);
	    comments.Add(comment); 
	    await WriteToJson(comments);
	  
    }

    public async Task DeleteAsync(int id)
    {
	    var comments = await ReadFromJson();
	    Comment? commentToRemove = comments.SingleOrDefault(p => p.Id == id); 
	    if (commentToRemove is null) 
	    { 
		    throw new InvalidOperationException($"Comment with ID '{id}' not found"); 
	    } 
	    comments.Remove(commentToRemove);
	    await WriteToJson(comments);
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
	    var comments = await ReadFromJson();
	    var commentToGet = comments.SingleOrDefault(p => p.Id == id);
	    if (commentToGet is null) 
	    { 
		    throw new InvalidOperationException($"Comment with ID '{id}' not found"); 
	    }

	    return commentToGet;
	    
    }
    
    
    
    public IQueryable<Comment> GetMany()
    {
	    string commentsAsJson = File.ReadAllTextAsync(filePath).Result;
	    List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
	    return comments.AsQueryable();

    }


}