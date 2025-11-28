namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; }
    
    // EFC stuff below
    public User User { get; set; }
    public Post Post { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    public Comment() {} // for EFC
}