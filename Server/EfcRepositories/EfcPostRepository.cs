using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcPostRepository : IPostRepository
{
    private readonly AppContext _context;

    public EfcPostRepository(AppContext context)
    {
        _context = context;
    }
    
    public async Task<Post> AddAsync(Post post)
    {
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        if (!await _context.Posts.AnyAsync(p => p.Id == post.Id))
        {
            throw new Exception($"Post with id {post.Id} not found");
        }
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Post? existing = await _context.Posts.SingleOrDefaultAsync(p => p.Id == id);
        if (existing == null)
        {
            throw new Exception($"Post with id {id} not found");
        }
        _context.Posts.Remove(existing);
        await _context.SaveChangesAsync();
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        Post? existing = await _context.Posts.SingleOrDefaultAsync(p => p.Id == id);
        if (existing == null)
        {
            throw new Exception($"Post with id {id} not found");
        }
        
        return existing;
    }

    public IQueryable<Post> GetMany()
    {
        return _context.Posts.AsQueryable();
    }
}