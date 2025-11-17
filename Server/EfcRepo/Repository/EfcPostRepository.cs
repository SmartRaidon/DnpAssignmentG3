using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EfcRepo.Repository;

public class EfcPostRepository: IPostRepository
{
    private readonly DataBaseContext context;

    public EfcPostRepository(DataBaseContext context)
    {
        this.context = context;
    }
    public async Task<Post> AddAsync(Post post)
    {
        await context.Posts.AddAsync(post);
        await context.SaveChangesAsync();
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        if (!(await context.Posts.AnyAsync(p=> p.Id == post.Id)))
        {
            throw new Exception("Post not found with id: " + post.Id);
        }
        context.Posts.Update(post);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Post? existing =  await context.Posts.FindAsync(id);
        if (existing == null)
        {
            throw new Exception("Post not found with id: " + id);
        }
        context.Posts.Remove(existing);
        await context.SaveChangesAsync();
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        Post? existing = await context.Posts.FindAsync(id);
        if (existing == null)
        {
            throw new Exception("Post not found with id: " + id);
        }
        return existing;
        
    }

    public IQueryable<Post> GetMany()
    {
        return context.Posts.AsQueryable();
    }
}