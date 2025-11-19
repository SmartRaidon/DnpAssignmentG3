namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    
    // EFC stuff below
    public User User { get; set; } // navigation for EFC (1:*)
    public int UserId { get; set; } // navigation for EFC (1:*)
    public Post() {} // for EFC
}