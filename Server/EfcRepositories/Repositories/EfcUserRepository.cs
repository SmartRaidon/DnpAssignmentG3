using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EfcRepositories.Repositories;

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
            throw new InvalidOperationException($"User with id {user.Id} not found");
        }

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        User? existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
        if (existingUser == null)
        {
            throw new InvalidOperationException($"User with id {id} not found");
        }

        _context.Users.Remove(existingUser);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetSingleAsync(int id)
    {
        User? userToGet = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
        if (userToGet is null)
        {
            throw new InvalidOperationException($"User with id {id} not found");
        }

        return userToGet;
    }

    public async Task<IQueryable<User>> GetManyAsync()
    {
        return await Task.FromResult(_context.Users.AsQueryable());
    }
}