using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EfcRepositories.Repositories;

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
            throw new InvalidOperationException($"Comment with id {comment.Id} not found");
        }

        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Comment? existing = await _context.Comments.SingleOrDefaultAsync(c => c.Id == id);
        if (existing == null)
        {
            throw new InvalidOperationException($"Comment with id {id} not found");
        }

        _context.Comments.Remove(existing);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Comment> GetSingleAsync(int id)
    {
        Comment? commentToGet = await _context.Comments.SingleOrDefaultAsync(c => c.Id == id);
        if (commentToGet is null)
        {
            throw new InvalidOperationException($"Comment with id {id} not found");
        }

        return commentToGet;
    }

    public async Task<IQueryable<Comment>> GetManyAsync()
    {
        return await Task.FromResult(_context.Comments.AsQueryable());
    }
    
    public Task<IQueryable<Comment>> GetManyByPostId(int postId)
    {
        IQueryable<Comment> commentById = _context.Comments.Where(c => c.PostId == postId);
        return Task.FromResult(commentById);
    }

}