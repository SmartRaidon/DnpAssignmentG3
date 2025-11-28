using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcUserRepository : IUserRepository
{
    private readonly AppContext _context;

    public EfcUserRepository(AppContext context)
    {
        _context = context;
    }
    
    public async Task<User> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        if (!await _context.Users.AnyAsync(u => u.Id == user.Id))
        {
            throw new Exception($"User with id {user.Id} not found");
        }
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        User? existing = await _context.Users.FindAsync(id);
        if (existing == null)
        {
            throw new Exception($"User with id {id} not found");
        }
        _context.Users.Remove(existing);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetSingleAsync(int id)
    {
        User? existing = await _context.Users.FindAsync(id);
        if (existing == null)
        {
            throw new Exception($"User with id {id} not found");
        }
        return existing;
    }

    public IQueryable<User> GetMany()
    {
        return _context.Users.AsQueryable();
    }
}