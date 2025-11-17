using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EfcRepo.Repository;

public class EfcCommentRepository : ICommentRepository
{
    private readonly DataBaseContext context;

    public EfcCommentRepository(DataBaseContext context)
    {
        this.context = context;
    }
    public async Task<Comment> AddAsync(Comment comment)
    {
        await context.Comments.AddAsync(comment);
        await context.SaveChangesAsync();
        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        if (!(await context.Comments.AnyAsync(c=> c.Id == comment.Id) ))
        {
            throw new Exception("Comment not found with id: " + comment.Id);
        }
        context.Comments.Update(comment);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Comment? existing = await context.Comments.FindAsync(id);
        if (existing == null)
        {
            throw new Exception("Comment not found with id: " + id);
        }
        context.Comments.Remove(existing);
        await context.SaveChangesAsync();
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        Comment? comment = await context.Comments.FindAsync(id);
        if (comment == null)
        {
            throw new Exception("Comment not found with id: " + id);
        }
        return comment;
    }

    public IQueryable<Comment> GetMany()
    {
        return context.Comments.AsQueryable();
    }
}