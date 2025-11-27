using Microsoft.EntityFrameworkCore;

namespace EfcRepositories;

public class AppContext : DbContext
{
    public DbSet<User> Users => Set<User>(); //each entity has a defined DbSet(table)
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //specify ue of Sqlite and db file name
        optionsBuilder.UseSqlite("Data Source=C:/Users/aless/RiderProjects/DnpAssignmentG3/Server/EfcRepositories/app.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<Post>().HasKey(u => u.Id);
        modelBuilder.Entity<Comment>().HasKey(u => u.Id);
    }
}