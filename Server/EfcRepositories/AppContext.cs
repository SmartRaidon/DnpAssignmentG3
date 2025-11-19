using Entities;
using Microsoft.EntityFrameworkCore;

namespace EfcRepositories;

public class AppContext : DbContext
{
    public DbSet<User> users => Set<User>(); //each entity has a defined DbSet(table)
    public DbSet<Post> posts => Set<Post>();
    public DbSet<Comment> comments => Set<Comment>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //specify ue of Sqlite and db file name
        optionsBuilder.UseSqlite("Data Source = app.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<Post>().HasKey(u => u.Id);
        modelBuilder.Entity<Comment>().HasKey(u => u.Id);
    }
}