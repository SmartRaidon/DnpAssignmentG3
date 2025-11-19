namespace Entities;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    
    // EFC stuff below
    public List<Post> Posts { get; set; } // navigation for EFC (1:*)
    public List<Comment> Comments { get; set; } // navigation for EFC (1:*)
    public User() {} // for EFC
}