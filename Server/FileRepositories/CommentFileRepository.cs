using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private static readonly string BASE_PATH =
        Path.Combine(AppContext.BaseDirectory, @"..\..\..\Files");
    private static readonly string PATH =
        Path.GetFullPath(Path.Combine(BASE_PATH, "comments.json"));
    
    public CommentFileRepository()
    {
        InitializeFileIfNotExists();
    }
    
    public async Task<Comment> AddAsync(Comment comment)
    {
        // getting comments as json list from the file path
        string jsonCommentList = await File.ReadAllTextAsync(PATH);
        // deserializing comments as list from json gathered from file path
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(jsonCommentList);
        
        // adding new comment
        comment.Id = comments.Any() ? comments.Max(p => p.Id) + 1 : 1;
        comments.Add(comment);
        
        // serializing again the comments list so that we add newly updated list into file path
        jsonCommentList = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(PATH, jsonCommentList);

        return await Task.FromResult(comment);
    }

    public async Task UpdateAsync(Comment comment)
    {
        // getting comments as json list from the file path
        string jsonCommentList = await File.ReadAllTextAsync(PATH);
        // deserializing comments as list from json gathered from file path
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(jsonCommentList);

        // finding the right comment
        Comment? existingComment = comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Comment with Id {comment.Id} not found");
        }
        // updating comment
        comments.Remove(existingComment);
        comments.Add(comment);
        
        // serializing again the comments list so that we add newly updated list into file path
        jsonCommentList = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(PATH, jsonCommentList);
    }

    public async Task DeleteAsync(int id)
    {
        // getting comments as json list from the file path
        string jsonCommentList = await File.ReadAllTextAsync(PATH);
        // deserializing comments as list from json gathered from file path
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(jsonCommentList);

        // finding the right comment
        Comment? CommentToRemove =
            comments.SingleOrDefault(c => c.Id == id);
        if (CommentToRemove is null)
        {
            throw new InvalidOperationException(
                $"Comment with Id {id} not found");
        }

        // removing comment
        comments.Remove(CommentToRemove);
        
        // serializing again the comments list so that we add newly updated list into file path
        jsonCommentList = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(PATH, jsonCommentList);
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        // getting comments as json list from the file path
        string jsonCommentList = await File.ReadAllTextAsync(PATH);
        // deserializing comments as list from json gathered from file path
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(jsonCommentList);

        // finding the right comment
        Comment? commentToGet = comments.SingleOrDefault(c => c.Id == id);
        if (commentToGet is null)
        {
            throw new InvalidOperationException(
                $"Comment with Id {id} not found");
        }
        
        return await Task.FromResult(commentToGet);
    }

    public async Task<IQueryable<Comment>> GetManyAsync()
    {
        // getting comments as json list from the file path
        string jsonCommentList = await File.ReadAllTextAsync(PATH);
        // deserializing comments as list from json gathered from file path
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(jsonCommentList);

        return await Task.FromResult(comments.AsQueryable());
    }

    private async void InitializeFileIfNotExists()
    {
        Directory.CreateDirectory(BASE_PATH);
        
        bool fileAlreadyExists = File.Exists(PATH);
        if (!fileAlreadyExists)
        {
            List<Comment> comments = new List<Comment>();
            string jsonComments = JsonSerializer.Serialize(comments);
            await File.WriteAllTextAsync(PATH, jsonComments);
        }
    }
}