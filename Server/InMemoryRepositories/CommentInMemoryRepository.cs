﻿using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    List<Comment> comments = new List<Comment>();
    
    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = comments.Any() ? comments.Max(p => p.Id) + 1 : 1; 
        comments.Add(comment); 
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment = comments.SingleOrDefault(p => p.Id == comment.Id); 
        if (existingComment is null) 
        { 
            throw new InvalidOperationException($"Comment with ID '{comment.Id}' not found"); 
        } 
        comments.Remove(existingComment);
        comments.Add(comment); 
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Comment? commentToRemove = comments.SingleOrDefault(p => p.Id == id); 
        if (commentToRemove is null) 
        { 
            throw new InvalidOperationException($"Comment with ID '{id}' not found"); 
        } 
        comments.Remove(commentToRemove); 
        return Task.CompletedTask; 
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? commentToRetrieve = comments.SingleOrDefault(p => p.Id == id); 
        if (commentToRetrieve is null) 
        { 
            throw new InvalidOperationException($"Comment with ID '{id}' not found"); 
        } 
        return Task.FromResult(commentToRetrieve);
    }

    public IQueryable<Comment> GetMany()
    {
        return comments.AsQueryable();
    }
}