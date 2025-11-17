using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EfcRepo.Repository;

public class EfcUserRepository : IUserRepository
{
    private readonly DataBaseContext context;
    public EfcUserRepository(DataBaseContext context)
        {
        this.context = context;
        }


    public async Task<User> AddAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        if (!(await context.Users.AnyAsync(u=> u.Id==user.Id)))
        {
            throw new Exception("User not found with id: " + user.Id);
        }
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        User? existing = await context.Users.FindAsync(id);
        if (existing == null)
        {
            throw new Exception("User not found with id: " + id);
        }
        context.Users.Remove(existing);
        await context.SaveChangesAsync();
    }

    public async Task<User> GetSingleAsync(int id)
    {
        User? user = await context.Users.FindAsync(id);
        if (user == null)
        {
            throw new Exception("User not found with id: " + id);
        }
        return user;
    }

    public async Task<User> GetSingelAsyncByUsername(string username)
    {
        User? userByUsername =  await context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (userByUsername == null)
        {
            throw new Exception("User not found with username: " + username);
        }
        return userByUsername;
        
    }

    public IQueryable<User> GetMany()
    {
        return context.Users.AsQueryable();
    }
}