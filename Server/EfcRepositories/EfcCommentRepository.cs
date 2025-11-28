using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcCommentRepository : ICommentRepository
{
    private readonly AppContext _context;

    public EfcCommentRepository(AppContext context)
    {
        _context = context;
    }
    
    public async Task<Comment> AddAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        if (!await _context.Comments.AnyAsync(c => c.Id == comment.Id))
        {
            throw new Exception($"Comment with id {comment.Id} doesn't exist");
        }
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Comment? existing = await _context.Comments.SingleOrDefaultAsync(c => c.Id == id);
        if (existing == null)
        {
            throw new Exception($"Comment with id {id} not found");
        }
        _context.Comments.Remove(existing);
        await _context.SaveChangesAsync();
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        // the Include parts are important
        Comment? existing = await _context.Comments
            .Include(c => c.Post)
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (existing == null)
        {
            throw new Exception($"Comment with id {id} not found");
        }
        return existing;
    }

    public IQueryable<Comment> GetMany()
    {
        return _context.Comments.AsQueryable();
    }
}