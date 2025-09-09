﻿using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    List<User> users = new List<User>();
    
    public Task<User> AddAsync(User user)
    {
        user.Id = users.Any() ? users.Max(p => p.Id) + 1 : 1; 
        users.Add(user); 
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        User? existingUser = users.SingleOrDefault(p => p.Id == user.Id); 
        if (existingUser is null) 
        { 
            throw new InvalidOperationException($"User with ID '{user.Id}' not found"); 
        } 
        users.Remove(existingUser);
        users.Add(user); 
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        User? userToRemove = users.SingleOrDefault(p => p.Id == id); 
        if (userToRemove is null) 
        { 
            throw new InvalidOperationException($"User with ID '{id}' not found"); 
        } 
        users.Remove(userToRemove); 
        return Task.CompletedTask; 
    }

    public Task<User> GetSingleAsync(int id)
    {
        User? userToRetrieve = users.SingleOrDefault(p => p.Id == id); 
        if (userToRetrieve is null) 
        { 
            throw new InvalidOperationException($"User with ID '{id}' not found"); 
        } 
        return Task.FromResult(userToRetrieve);
    }

    public IQueryable<User> GetMany()
    {
        return users.AsQueryable();
    }
}